using Simson.Chat.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Simson.Chat.WebClient.Options;

namespace Simson.Chat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatContext _context;
        private readonly ChatOptions _options;
        private readonly ILogger<ChatHub> _logger;
        private readonly int _messagesCount;
        private readonly string _userNameKey = "username";

        public ChatHub(IChatContext context, IOptions<ChatOptions> options, ILogger<ChatHub> logger)
        {
            _context = context;
            _options = options.Value;
            _logger = logger;
            _messagesCount = _options.LastMessagesCount;
        }

        public async Task<Context> Login(string userName)
        {
            _logger.LogInformation($"{nameof(Login)}, username {userName}");

            try
            {
                var result = new Context();
                var loginResult = await _context.LoginAsync(userName, default);
                _logger.LogInformation($"{nameof(Login)}, username {userName} - {loginResult.Reason}");
                result.Reason = loginResult.Reason;

                if (loginResult.Success)
                {
                    Context.Items[_userNameKey] = userName;
                    var activeUsers = (await _context.GetUsersAsync(default))
                            .Where(x => x.Status == UserStatus.Online)
                            .ToList();
                    var lastMessages = (await _context.GetMessagesAsync(_messagesCount, default))
                            .ToList();
                    result.Users = activeUsers;
                    result.Messages = lastMessages;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(Login)}, username {userName}");
                throw;
            }
        }

        public async Task Logout(string userName)
        {
            _logger.LogInformation($"{nameof(Logout)}, username {userName}");

            try
            {
                await _context.LogoutAsync(userName, default);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(Logout)}, username {userName}");
                throw;
            }
        }

        public async Task SendMessage(Message message)
        {
            _logger.LogInformation($"{nameof(SendMessage)}, username {message.User.Name}");

            try
            {
                await _context.AddMessageAsync(message, default);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(SendMessage)}, username {message.User.Name}");
                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.Items.TryGetValue(_userNameKey, out var result) && result is string userName)
            {
                _logger.LogInformation($"{nameof(OnDisconnectedAsync)} username {userName} => {nameof(Logout)}");
                await _context.LogoutAsync(userName, default);
            }
        }
    }
}
