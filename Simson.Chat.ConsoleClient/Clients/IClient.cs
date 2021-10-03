using System;
using System.Threading;
using System.Threading.Tasks;

namespace Simson.Chat.ConsoleClient.Clients
{
    interface IClient
    {
        Task<bool> TryConnect(Uri uri, CancellationToken cancellationToken);
    }
}
