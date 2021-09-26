using Simson.Chat.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Simson.Chat.Hubs
{
    public class ChatNotifier : IHostedService
    {
        private readonly IChatContext _chat;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatNotifier(IChatContext chatContext, IHubContext<ChatHub> hubContext)
        {
            _chat = chatContext;
            _hubContext = hubContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _chat.MessageReceived += OnMessageReceived;
            _chat.UserStatusChanged += OnUserStatusChanged;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _chat.MessageReceived -= OnMessageReceived;
            _chat.UserStatusChanged -= OnUserStatusChanged;
            return Task.CompletedTask;
        }

        private void OnMessageReceived(Message message)
        {
            _hubContext.Clients.All.SendAsync("OnMessageReceived", message);
        }

        private void OnUserStatusChanged(User user)
        {
            _hubContext.Clients.All.SendAsync("OnUserStatusChanged", user);
        }
    }
}
