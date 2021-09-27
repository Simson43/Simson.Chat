using Simson.Chat.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Simson.Chat
{
    public interface IChatContext
    {
        event Action<Message> MessageReceived;
        event Action<User> UserStatusChanged;

        Task<LoginResult> LoginAsync(string userName, CancellationToken cancellationToken);
        Task LogoutAsync(string userName, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Message>> GetMessagesAsync(int count, CancellationToken cancellationToken);
        Task AddMessageAsync(Message message, CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}
