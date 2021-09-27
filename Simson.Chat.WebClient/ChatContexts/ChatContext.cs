using Microsoft.Extensions.Hosting;
using Simson.Chat.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Simson.Chat
{
    public class ChatContext : IChatContext
    {
        public event Action<Message> MessageReceived;
        public event Action<User> UserStatusChanged;

        private readonly IUserStore _userStore;
        private readonly IMessageStore _messageStore;
        private readonly IHostApplicationLifetime _application;


        public ChatContext(IUserStore userStore, IMessageStore messageStore, IHostApplicationLifetime application)
        {
            _userStore = userStore;
            _messageStore = messageStore;
            _application = application;
        }

        public Task<IEnumerable<Message>> GetMessagesAsync(int count, CancellationToken cancellationToken)
        {
            return _messageStore.GetLastAsync(count, cancellationToken);
        }

        public Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
        {
            return _userStore.GetAllAsync(cancellationToken);
        }

        public async Task<LoginResult> LoginAsync(string userName, CancellationToken cancellationToken)
        {
            LoginResult result;

            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    result = new LoginResult(LoginReason.IncorrectUserName);
                else if (await _userStore.ContainsAsync(userName, cancellationToken))
                    result = new LoginResult(LoginReason.AlreadyExists);
                else
                {
                    var user = new User(userName, UserStatus.Online);
                    await _userStore.AddAsync(user, cancellationToken);
                    result = LoginResult.GetSuccess;
                    UserStatusChanged?.Invoke(user);
                }
            }
            catch 
            {
                result = new LoginResult(LoginReason.UnknownError);
            }

            return result;
        }

        public async Task LogoutAsync(string userName, CancellationToken cancellationToken)
        {
            var user = await _userStore.RemoveAsync(userName, cancellationToken);
            user.Status = UserStatus.Offline;
            UserStatusChanged?.Invoke(user);
        }

        public async Task AddMessageAsync(Message message, CancellationToken cancellationToken)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.Text) || !await _userStore.ContainsAsync(message.User.Name, cancellationToken))
                return;
            await _messageStore.AddAsync(message, cancellationToken);
            MessageReceived?.Invoke(message);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _application.StopApplication();
            return Task.CompletedTask;
        }
    }
}
