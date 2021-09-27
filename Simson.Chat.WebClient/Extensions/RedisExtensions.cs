using StackExchange.Redis;
using System.Text.Json;

namespace Simson.Chat.WebClient.Extensions
{
    public static class RedisExtensions
    {
        public static RedisValue ToRedisValue<T>(this T value)
        {
            if (value.Equals(default))
                return default;
            return JsonSerializer.Serialize(value);
        }

        public static T FromRedisValue<T>(this RedisValue redisValue)
        {
            if (redisValue.Equals(default))
                return default;
            return JsonSerializer.Deserialize<T>(redisValue);
        }
    }
}
