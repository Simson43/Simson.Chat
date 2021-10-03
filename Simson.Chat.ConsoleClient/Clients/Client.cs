using Grpc.Net.Client;
using Simson.Chat.gRPC;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Simson.Chat.gRPC.Chat;

namespace Simson.Chat.ConsoleClient.Clients
{
    class Client : IClient, IDisposable
    {
        private GrpcChannel _channel;
        private ChatClient _client;
        private bool _disposed;

        public async Task<bool> TryConnect(Uri uri, CancellationToken cancellationToken)
        {
            try
            {
                _channel = GrpcChannel.ForAddress(uri.AbsoluteUri);
                _client = new ChatClient(_channel);
                if (await CheckAsync(_client, cancellationToken))
                    return true;
            }
            catch
            { }

            Dispose();
            return false;
        }

        private async Task<bool> CheckAsync(ChatClient client, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Checking server...");
                await client.CheckAsync(new HealthCheckRequest(), cancellationToken: cancellationToken);
                Console.WriteLine("Server is healthy");
                return true;
            }
            catch
            {
                Console.WriteLine("Server not found");
                return false;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _channel?.Dispose();
                _disposed = true;
            }
        }

        ~Client()
        {
            Dispose();
        }
    }
}
