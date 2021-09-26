using AveriaTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AveriaTest
{
    public interface IMessageStore
    {
        Task<IEnumerable<Message>> GetAll();
        Task Add(Message message);
    }
}
