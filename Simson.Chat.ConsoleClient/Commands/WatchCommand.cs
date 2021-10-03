using Grpc.Core;
using Simson.Chat.gRPC;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Simson.Chat.gRPC.Chat;

namespace ConsoleClient
{
    internal class WatchCommand : ICommand
    {
        public string Key { get; } = "/watch";
        public string Description { get; } = $"Watch the chat (last {_lastMessagesCount} messages)";

        public const int _lastMessagesCount = 20;
        private readonly ChatClient _client;

        public WatchCommand(ChatClient client)
        {
            _client = client;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            StartWriteMessages();
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var stream = _client.GetStreamMessages(new GetStreamMessagesRequest { Count = _lastMessagesCount })
                .ResponseStream.ReadAllAsync().WithCancellation(cts.Token);
            var streaming = StreamMessages(stream);
            WaitCancelling(cts);
            ConsoleHelper.WriteSeparator();
        }

        private static void WaitCancelling(CancellationTokenSource cts)
        {
            Console.ReadKey();
            Console.WriteLine();
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
            ConsoleHelper.WriteSeparator();
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
    }
}
