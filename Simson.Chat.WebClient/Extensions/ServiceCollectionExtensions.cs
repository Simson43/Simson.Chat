using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Simson.Chat.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemoryStores(this IServiceCollection services)
        {
            services.AddSingleton<IUserStore, InMemoryUserStore>();
            services.AddSingleton<IMessageStore, InMemoryMessageStore>();
            return services;
        }

        public static IServiceCollection AddRedisStores(this IServiceCollection services, string address)
        {
            var multiplexer = ConnectionMultiplexer.Connect(address);
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);

            services.AddSingleton<IUserStore, RedisUserStore>();
            services.AddSingleton<IMessageStore, RedisMessageStore>();
            return services;
        }
    }
}
