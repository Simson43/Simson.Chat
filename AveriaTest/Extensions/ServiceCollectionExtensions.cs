using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AveriaTest.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemoryStores(this IServiceCollection services)
        {
            services.AddSingleton<IUserStore, InMemoryUserStore>();
            services.AddSingleton<IMessageStore, InMemoryMessageStore>();
            return services;
        }

        public static IServiceCollection AddRedisStores(this IServiceCollection services)
        {
            var multiplexer = ConnectionMultiplexer.Connect("localhost");
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);

            services.AddSingleton<IUserStore, RedisUserStore>();
            services.AddSingleton<IMessageStore, RedisMessageStore>();
            return services;
        }
    }
}
