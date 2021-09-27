using Grpc.Core;
using Grpc.Net.Client;
using Simson.Chat.gRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleClient
{
    enum CommandType
    {
        Stop,
        GetUsers,
        Watch
    }

    class Command
    {
        internal CommandType CommandType { get; set; }
        internal string Message { get; set; }
    }

    class Program
    {
        private readonly static int _lastMessagesCount = 20;
        private static Dictionary<string, Command> _commands = new Dictionary<string, Command>
        {
            ["/stop"] = new Command { CommandType = CommandType.Stop, Message = "To stop server process" },
            ["/ls"] = new Command { CommandType = CommandType.GetUsers, Message = "Get list of online Users" },
            ["/watch"] = new Command { CommandType = CommandType.Watch, Message = $"Watch the chat (last {_lastMessagesCount} messages)" }
        };

        static async Task Main(string[] args)
        {
            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                cts.Cancel();
            };

            await RunAsync(cts.Token);
        }

        static async Task RunAsync(CancellationToken cancellationToken)
        {
            GrpcChannel channel;
            Chat.ChatClient client = null;

            while (!cancellationToken.IsCancellationRequested)
            {
                var address = GetAddress();
                channel = GrpcChannel.ForAddress(address);
                client = new Chat.ChatClient(channel);
                if (await CheckAsync(client, cancellationToken))
                    break;
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var command = GetCommand();
                    switch (command)
                    {
                        case CommandType.Stop:
                            await StopAsync(client, cancellationToken);
                            return;
                        case CommandType.GetUsers:
                            await GetUsersAsync(client, cancellationToken);
                            break;
                        case CommandType.Watch:
                            await WatchMessagesAsync(client, cancellationToken);
                            break;
                    }
                }
                catch
                {

                }
            }
        }

        private static async Task<bool> CheckAsync(Chat.ChatClient client, CancellationToken cancellationToken)
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

        private static async Task StopAsync(Chat.ChatClient client, CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping server...");

            await client.StopAsync(new StopRequest(), cancellationToken: cancellationToken);

            Console.WriteLine("Server is stopped");
        }

        private static async Task GetUsersAsync(Chat.ChatClient client, CancellationToken cancellationToken)
        {
            Console.WriteLine("Users Online:");
            WriteSeparator();
            var stream = client.GetUsers(new GetUsersRequest())
                .ResponseStream.ReadAllAsync().WithCancellation(cancellationToken);
            var index = 1;
            await foreach (var user in stream)
            {
                Console.WriteLine($"{index++} - {user.Name}");
            }
            WriteSeparator();
        }

        private static async Task WatchMessagesAsync(Chat.ChatClient client, CancellationToken cancellationToken)
        {
            StartWriteMessages();
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var stream = client.GetStreamMessages(new GetStreamMessagesRequest { Count = _lastMessagesCount })
                .ResponseStream.ReadAllAsync().WithCancellation(cts.Token);
            var streaming = StreamMessages(stream);
            Console.ReadKey();
            Console.WriteLine();
            WriteSeparator();
            cts.Cancel();
        }

        private static async Task StreamMessages(System.Runtime.CompilerServices.ConfiguredCancelableAsyncEnumerable<Message> stream)
        {
            var messages = new Queue<Message>(_lastMessagesCount);
            await foreach (var message in stream)
            {
                if (messages.Count == _lastMessagesCount)
                {
                    messages.Dequeue();
                    StartWriteMessages(messages);
                }
                messages.Enqueue(message);
                WriteMessage(message);
            }
        }

        private static void StartWriteMessages(IEnumerable<Message> messages = null)
        {
            Console.Clear();
            Console.WriteLine($"Watching last {_lastMessagesCount} messages...");
            Console.WriteLine($"Press any key to cancel");
            WriteSeparator();
            if (messages != null)
            {
                foreach (var msg in messages)
                {
                    WriteMessage(msg);
                }
            }
        }

        private static void WriteMessage(Message message)
        {
            Console.WriteLine($"[{message.Date: dd.MM.yy HH:mm}] {message.UserName}: {message.Text}");
        }

        private static CommandType GetCommand()
        {
            foreach (var command in _commands)
            {
                Console.WriteLine($"{command.Key} - {command.Value.Message}");
            }

            Command result = null;
            string input = null;
            do
            {
                Console.Write("Enter command: ");
                input = Console.ReadLine();
                if (input == null)
                    throw new Exception();
            } while (!_commands.TryGetValue(input, out result));

            return result.CommandType;
        }

        private static string GetAddress()
        {
            var protocol = "https";
            Console.WriteLine($"Used protocol: {protocol}");
            var ip = GetIp();
            var port = GetPort();
            var result = $"{protocol}://{ip}:{port}";
            Console.WriteLine($"Used address: {result}");
            return result;
        }

        private static string GetIp()
        {
            string result;
            do
            {
                Console.Write("IP: ");
                result = Console.ReadLine().ToLower();
            } while (result != "localhost" && !IPAddress.TryParse(result, out _));
            return result;
        }

        private static string GetPort()
        {
            string result;
            do
            {
                Console.Write("Port: ");
                result = Console.ReadLine();
            } while (!ushort.TryParse(result, out _));
            return result;
        }

        private static void WriteSeparator()
        {
            Console.WriteLine("--------------------------------");
        }
    }
}
