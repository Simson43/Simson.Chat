using Simson.Chat.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Simson.Chat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatContext _context;
        private readonly ILogger<ChatHub> _logger;
        private readonly int _messagesCount = 50;
        private readonly string _userNameKey = "username";

        public ChatHub(IChatContext context, ILogger<ChatHub> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Context> Login(string userName)
        {
            _logger.LogInformation($"{nameof(Login)}, username {userName}");

            var result = new Context();
            var loginResult = await _context.Login(userName);
            result.Reason = loginResult.Reason;

            if (loginResult.Success)
            {
                Context.Items[_userNameKey] = userName;
                var activeUsers = (await _context.GetUsers())
                        .Where(x => x.Status == UserStatus.Online)
                        .ToList();
                var lastMessages = (await _context.GetMessages())
                        .TakeLast(_messagesCount)
                        .ToList();
                result.Users = activeUsers;
                result.Messages = lastMessages;
            }

            return result;
        }

        public async Task Logout(string userName)
        {
            _logger.LogInformation($"{nameof(Logout)}, username {userName}");

            await _context.Logout(userName);
        }

        public async Task SendMessage(Message message)
        {
            _logger.LogInformation($"{nameof(SendMessage)}, username {message.User}");

            await _context.AddMessage(message);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.Items.TryGetValue(_userNameKey, out var result) && result is string userName)
            {
                _logger.LogInformation($"{nameof(OnDisconnectedAsync)} username {userName} => {nameof(Logout)}");
                await _context.Logout(userName);
            }
        }
    }
}
