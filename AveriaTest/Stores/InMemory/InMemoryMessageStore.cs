using AveriaTest.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AveriaTest
{
    public class InMemoryMessageStore : IMessageStore
    {
        private readonly ConcurrentQueue<Message> _messages = new ConcurrentQueue<Message>();

        Task<IEnumerable<Message>> IMessageStore.GetAll()
        {
            return Task.FromResult(_messages.AsEnumerable()); // TODO: check
        }

        public Task Add(Message message)
        {
            _messages.Enqueue(message);
            return Task.CompletedTask;
        }
    }
}
