using Simson.Chat.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simson.Chat
{
    public interface IUserStore
    {
        Task<IEnumerable<User>> GetAll();
        Task<bool> Contains(string userName);
        Task Add(User entity);
        Task<User> Remove(string userName);
    }
}
