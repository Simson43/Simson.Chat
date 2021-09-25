using AveriaTest.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AveriaTest
{
    public class MemoryChatContext : IChatContext
    {
        private readonly List<Message> _messages = new List<Message>();
        private readonly ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>();

        public event Action<Message> MessageReceived;
        public event Action<User> UserStatusChanged;

        public Task<IEnumerable<Message>> GetMessages()
        {
            lock (_messages)
            {
                return Task.FromResult(_messages.ToList().AsEnumerable());
            }
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            return Task.FromResult(_users.Values.AsEnumerable());
        }

        public Task<LoginResult> Login(string userName)
        {
            LoginResult result;

            try
            {
                if (string.IsNullOrEmpty(userName))
                    result = new LoginResult(LoginReason.IncorrectUserName);
                else if (_users.ContainsKey(userName))
                    result = new LoginResult(LoginReason.AlreadyExists);
                else
                {
                    var user = new User(userName, UserStatus.Online);
                    _users[userName] = user;
                    result = LoginResult.GetSuccess;
                    UserStatusChanged?.Invoke(user);
                }
            }
            catch (Exception ex)
            {
                result = new LoginResult(LoginReason.UnknownError);
            }

            return Task.FromResult(result);
        }

        public Task Logout(string userName)
        {
            _users.Remove(userName, out var user);
            user.Status = UserStatus.Offline;
            UserStatusChanged?.Invoke(user);
            return Task.CompletedTask;
        }

        public Task AddMessage(Message message)
        {
            lock (_messages)
            {
                _messages.Add(message);
            }
            MessageReceived?.Invoke(message);
            return Task.CompletedTask;
        }
    }
}
