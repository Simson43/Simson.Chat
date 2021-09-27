using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Simson.Chat.gRPC
{
    public class ChatService : Chat.ChatBase
    {
        private readonly IChatContext _chatContext;
        private readonly ILogger<ChatService> _logger;

        public ChatService(IChatContext chatContext, ILogger<ChatService> logger)
        {
            _chatContext = chatContext;
            _logger = logger;
        }

        public override async Task GetUsers(GetUsersRequest request, IServerStreamWriter<User> responseStream, ServerCallContext context)
        {
            _logger.LogInformation(nameof(GetUsers));

            var users = await _chatContext.GetUsersAsync(context.CancellationToken);
            foreach (var user in users)
            {
                await responseStream.WriteAsync(new User { Name = user.Name });
            }
        }

        public override async Task GetStreamMessages(GetStreamMessagesRequest request, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            _logger.LogInformation($"{nameof(GetStreamMessages)} start: Count - {request.Count}");

            void OnMessageReceived(Models.Message message)
            {
                SendMessage(responseStream, message).GetAwaiter().GetResult();
            }

            try
            {
                // Send last existent messages
                var messages = await _chatContext.GetMessagesAsync(request.Count, context.CancellationToken);
                foreach (var message in messages)
                {
                    await SendMessage(responseStream, message);
                }

                // Send new messages
                var tcs = new TaskCompletionSource();
                using var cr = context.CancellationToken.Register(() => tcs.TrySetResult());

                _chatContext.MessageReceived += OnMessageReceived;

                await tcs.Task;

                _chatContext.MessageReceived -= OnMessageReceived;
            }
            finally
            {
                _logger.LogInformation($"{nameof(GetStreamMessages)} finished");
            }
        }

        public override Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HealthCheckResponse());
        }

        public override async Task<StopResponse> Stop(StopRequest request, ServerCallContext context)
        {
            await _chatContext.StopAsync(context.CancellationToken);
            return new StopResponse();
        }

        private static async Task SendMessage(IServerStreamWriter<Message> responseStream, Models.Message message)
        {
            await responseStream.WriteAsync(new Message { Date = message.Date.ToString(), UserName = message.User.Name, Text = message.Text });
        }
    }
}
