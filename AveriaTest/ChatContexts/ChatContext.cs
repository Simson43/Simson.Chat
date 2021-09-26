using AveriaTest.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AveriaTest
{
    public class ChatContext : IChatContext
    {
        public event Action<Message> MessageReceived;
        public event Action<User> UserStatusChanged;

        private readonly IUserStore _userStore;
        private readonly IMessageStore _messageStore;


        public ChatContext(IUserStore userStore, IMessageStore messageStore)
        {
            _userStore = userStore;
            _messageStore = messageStore;
        }

        public Task<IEnumerable<Message>> GetMessages()
        {
            return _messageStore.GetAll();
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            return _userStore.GetAll();
        }

        public async Task<LoginResult> Login(string userName)
        {
            LoginResult result;

            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    result = new LoginResult(LoginReason.IncorrectUserName);
                else if (await _userStore.Contains(userName))
                    result = new LoginResult(LoginReason.AlreadyExists);
                else
                {
                    var user = new User(userName, UserStatus.Online);
                    await _userStore.Add(user);
                    result = LoginResult.GetSuccess;
                    UserStatusChanged?.Invoke(user);
                }
            }
            catch (Exception ex)
            {
                result = new LoginResult(LoginReason.UnknownError);
            }

            return result;
        }

        public async Task Logout(string userName)
        {
            var user = await _userStore.Remove(userName);
            user.Status = UserStatus.Offline;
            UserStatusChanged?.Invoke(user);
        }

        public async Task AddMessage(Message message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.Text) || !await _userStore.Contains(message.User.Name))
                return;
            await _messageStore.Add(message);
            MessageReceived?.Invoke(message);
        }
    }
}
