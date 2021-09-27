using Simson.Chat.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Simson.Chat
{
    public interface IUserStore
    {
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> ContainsAsync(string userName, CancellationToken cancellationToken);
        Task AddAsync(User entity, CancellationToken cancellationToken);
        Task<User> RemoveAsync(string userName, CancellationToken cancellationToken);
    }
}
