using Simson.Chat.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Simson.Chat
{
    public interface IMessageStore
    {
        Task<IEnumerable<Message>> GetLastAsync(int count, CancellationToken cancellationToken);
        Task AddAsync(Message message, CancellationToken cancellationToken);
    }
}
