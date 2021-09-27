using Simson.Chat.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Simson.Chat
{
    public class RedisMessageStore : IMessageStore
    {
        private readonly ConnectionMultiplexer _multiplexer;

        private readonly string _key = "messages";

        public RedisMessageStore(ConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
        }

        public Task AddAsync(Message message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> GetLastAsync(int count, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
