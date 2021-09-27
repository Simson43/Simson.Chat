using StackExchange.Redis;

namespace Simson.Chat
{
    public abstract class RedisStore
    {
        private readonly IConnectionMultiplexer _multiplexer;
        protected IDatabase Db => _multiplexer.GetDatabase();
        protected readonly string Key;

        public RedisStore(string key, IConnectionMultiplexer multiplexer)
        {
            Key = key;
            _multiplexer = multiplexer;
        }
    }
}
