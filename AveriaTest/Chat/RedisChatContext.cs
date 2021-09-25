using AveriaTest.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AveriaTest
{
    public class RedisChatContext : IChatContext
    {
        private readonly ConnectionMultiplexer _multiplexer;

        private readonly string _messagesKey = "messages";

        public RedisChatContext(ConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
        }

        public event Action<Message> MessageReceived;
        public event Action<User> UserStatusChanged;

        public Task AddMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> GetMessages()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public Task<LoginResult> Login(string userName)
        {
            throw new NotImplementedException();
        }

        public Task Logout(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
