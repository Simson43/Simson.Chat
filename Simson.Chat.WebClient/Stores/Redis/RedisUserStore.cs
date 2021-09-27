using Simson.Chat.Models;
using Simson.Chat.WebClient.Extensions;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Simson.Chat
{
    public class RedisUserStore : RedisStore, IUserStore
    {
        public RedisUserStore(IConnectionMultiplexer multiplexer)
            : base("users", multiplexer)
        {
            Db.KeyDelete(Key);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await Db.ListRightPushAsync(Key, user.ToRedisValue());
        }

        public async Task<bool> ContainsAsync(string userName, CancellationToken cancellationToken)
        {
            return await GetAsync(userName, cancellationToken) != null;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return (await Db.ListRangeAsync(Key))
                .Select(x => x.FromRedisValue<User>());
        }

        public async Task<User> RemoveAsync(string userName, CancellationToken cancellationToken)
        {
            var result = await GetAsync(userName, cancellationToken);
            if (result != null)
                await Db.ListRemoveAsync(Key, result.ToRedisValue());
            return result;

        }

        private async Task<User> GetAsync(string userName, CancellationToken cancellationToken)
        {
            var count = Db.ListLength(Key);
            for (int i = 0; i < count; i++)
            {
                var item = (await Db.ListGetByIndexAsync(Key, i)).FromRedisValue<User>();
                if (item.Name == userName)
                    return item;
            }
            return null;
        }
    }
}
