using AveriaTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AveriaTest
{
    public interface IUserStore
    {
        Task<IEnumerable<User>> GetAll();
        Task<bool> Contains(string userName);
        Task Add(User entity);
        Task<User> Remove(string userName);
    }
}
