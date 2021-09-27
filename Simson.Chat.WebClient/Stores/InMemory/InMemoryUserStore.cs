using Simson.Chat.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Simson.Chat
{
    public class InMemoryUserStore : IUserStore
    {
        private readonly ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>();

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_users.Values.AsEnumerable());
        }

        public Task AddAsync(User user, CancellationToken cancellationToken)
        {
            _users[user.Name] = user;
            return Task.CompletedTask;
        }

        public Task<bool> ContainsAsync(string userName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_users.ContainsKey(userName));
        }

        public Task<User> RemoveAsync(string userName, CancellationToken cancellationToken)
        {
            _users.Remove(userName, out var user);
            return Task.FromResult(user);
        }
    }
}
