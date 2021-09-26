using Simson.Chat.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simson.Chat
{
    public interface IMessageStore
    {
        Task<IEnumerable<Message>> GetAll();
        Task Add(Message message);
    }
}
