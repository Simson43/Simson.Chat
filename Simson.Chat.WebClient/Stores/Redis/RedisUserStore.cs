using Simson.Chat.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Simson.Chat
{
    public class RedisUserStore : IUserStore
    {
        private readonly ConnectionMultiplexer _multiplexer;

        private readonly string _key = "users";

        public RedisUserStore(ConnectionMultiplexer multiplexer, CancellationToken cancellationToken)
        {
            _multiplexer = multiplexer;
        }

        public Task AddAsync(User entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ContainsAsync(string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User> RemoveAsync(string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
