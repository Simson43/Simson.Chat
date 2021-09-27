using Simson.Chat.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Simson.Chat
{
    public class InMemoryMessageStore : IMessageStore
    {
        private readonly ConcurrentQueue<Message> _messages = new ConcurrentQueue<Message>();

        public Task<IEnumerable<Message>> GetLastAsync(int count, CancellationToken cancellationToken)
        {
            return Task.FromResult(_messages.TakeLast(count));
        }

        public Task AddAsync(Message message, CancellationToken cancellationToken)
        {
            _messages.Enqueue(message);
            return Task.CompletedTask;
        }
    }
}
