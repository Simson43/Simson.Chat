using Simson.Chat.gRPC;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Simson.Chat.gRPC.Chat;

namespace ConsoleClient
{
    internal class StopCommand : ICommand
    {
        public string Key { get; } = "/stop";
        public string Description { get; } = "To stop server process";

        private readonly ChatClient _client;

        public StopCommand(ChatClient client)
        {
            _client = client;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping server...");

            await _client.StopAsync(new StopRequest(), cancellationToken: cancellationToken);

            Console.WriteLine("Server is stopped");
        }
    }
}
