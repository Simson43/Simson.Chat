using AveriaTest.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AveriaTest
{
    public class RedisMessageStore : IMessageStore
    {
        private readonly ConnectionMultiplexer _multiplexer;

        private readonly string _key = "messages";

        public RedisMessageStore(ConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
        }

        public Task Add(Message message)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
