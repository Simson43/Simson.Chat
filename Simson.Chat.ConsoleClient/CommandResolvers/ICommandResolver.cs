using ConsoleClient;
using System.Collections.Generic;

namespace Simson.Chat.ConsoleClient
{
    interface ICommandResolver
    {
        bool TryGetCommand(string key, out ICommand command);
        IEnumerable<ICommand> GetCommands();
    }
}
