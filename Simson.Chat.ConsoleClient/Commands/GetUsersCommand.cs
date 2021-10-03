using Grpc.Core;
using Simson.Chat.gRPC;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Simson.Chat.gRPC.Chat;

namespace ConsoleClient
{
    internal class GetUsersCommand : ICommand
    {
        public string Key { get; } = "/ls";
        public string Description { get; } = "Get list of online Users";

        private readonly ChatClient _client;

        public GetUsersCommand(ChatClient client)
        {
            _client = client;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("Users Online:");
            ConsoleHelper.WriteSeparator();
            var stream = _client.GetUsers(new GetUsersRequest())
                .ResponseStream.ReadAllAsync().WithCancellation(cancellationToken);
            var index = 1;
            await foreach (var user in stream)
            {
                Console.WriteLine($"{index++} - {user.Name}");
            }
            ConsoleHelper.WriteSeparator();
        }
    }
}
