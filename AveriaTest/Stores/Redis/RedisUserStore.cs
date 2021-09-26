using AveriaTest.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AveriaTest
{
    public class RedisUserStore : IUserStore
    {
        private readonly ConnectionMultiplexer _multiplexer;

        private readonly string _key = "users";

        public RedisUserStore(ConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
        }

        public Task Add(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Contains(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<User> Remove(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
