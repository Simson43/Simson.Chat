using Microsoft.Extensions.DependencyInjection;
using Simson.Chat.ConsoleClient;
using Simson.Chat.ConsoleClient.Clients;
using Simson.Microsoft.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                cts.Cancel();
            };

            await RunAsync(cts.Token);
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddImplementations<ICommand>(ServiceLifetime.Singleton);
            services.AddSingleton<ICommandResolver, CommandResolver>();
            services.AddSingleton<IClient, Client>();
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        static async Task RunAsync(CancellationToken cancellationToken)
        {
            var serviceProvider = ConfigureServices();

            var client = serviceProvider.GetRequiredService<IClient>();
            while (!cancellationToken.IsCancellationRequested)
            {
                var address = ConsoleHelper.GetAddress();
                if (await client.TryConnect(address, cancellationToken))
                    break;
            }

            var commandResolver = serviceProvider.GetRequiredService<ICommandResolver>();
            foreach (var item in commandResolver.GetCommands())
                Console.WriteLine($"{item.Key} - {item.Description}");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Console.Write("Enter command: ");
                    var input = Console.ReadLine();
                    if (commandResolver.TryGetCommand(input, out var command))
                        await command.Execute(cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occured: {ex.Message}");
                }
            }
        }
    }
}
