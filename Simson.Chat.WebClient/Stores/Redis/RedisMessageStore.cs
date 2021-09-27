using Simson.Chat.Models;
using Simson.Chat.WebClient.Extensions;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Simson.Chat
{
    public class RedisMessageStore : RedisStore, IMessageStore
    {
        public RedisMessageStore(IConnectionMultiplexer multiplexer)
            : base("messages", multiplexer)
        { }

        public async Task AddAsync(Message message, CancellationToken cancellationToken)
        {
            await Db.ListRightPushAsync(Key, message.ToRedisValue());
        }

        public async Task<IEnumerable<Message>> GetLastAsync(int count, CancellationToken cancellationToken)
        {
            var length = await Db.ListLengthAsync(Key);
            return (await Db.ListRangeAsync(Key, length - count))
                .Select(x => x.FromRedisValue<Message>());
        }
    }
}
