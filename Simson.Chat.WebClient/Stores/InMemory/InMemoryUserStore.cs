using Simson.Chat.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simson.Chat
{
    public class InMemoryUserStore : IUserStore
    {
        private readonly ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>();

        public Task<IEnumerable<User>> GetAll()
        {
            return Task.FromResult(_users.Values.AsEnumerable()); // TODO: check
        }

        public Task Add(User user)
        {
            _users[user.Name] = user;
            return Task.CompletedTask;
        }

        public Task<bool> Contains(string userName)
        {
            return Task.FromResult(_users.ContainsKey(userName));
        }

        public Task<User> Remove(string userName)
        {
            _users.Remove(userName, out var user);
            return Task.FromResult(user);
        }
    }
}
