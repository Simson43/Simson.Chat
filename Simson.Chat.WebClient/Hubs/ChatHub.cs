﻿using Simson.Chat.Models;
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
        private readonly int _messagesCount = 50; // TODO: to appsettings.json
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
            var loginResult = await _context.LoginAsync(userName, default);
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

        public async Task Logout(string userName)
        {
            _logger.LogInformation($"{nameof(Logout)}, username {userName}");

            await _context.LogoutAsync(userName, default);
        }

        public async Task SendMessage(Message message)
        {
            _logger.LogInformation($"{nameof(SendMessage)}, username {message.User.Name}");

            await _context.AddMessageAsync(message, default);
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
