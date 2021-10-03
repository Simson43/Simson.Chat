using System.Threading;
using System.Threading.Tasks;

namespace ConsoleClient
{
    internal interface ICommand
    {
        string Key { get; }
        string Description { get; }

        Task Execute(CancellationToken cancellationToken);
    }
}