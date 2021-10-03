using ConsoleClient;
using System.Collections.Generic;
using System.Linq;

namespace Simson.Chat.ConsoleClient
{
    class CommandResolver : ICommandResolver
    {
        private readonly Dictionary<string, ICommand> _commands;

        public CommandResolver(IEnumerable<ICommand> commands)
        {
            _commands = commands
                .ToDictionary(x => x.Key, x => x);
        }

        public bool TryGetCommand(string key, out ICommand command)
        {
            if (key != null && _commands.TryGetValue(key, out command))
                return true;
            command = null;
            return false;
        }

        public IEnumerable<ICommand> GetCommands()
        {
            return _commands.Values;
        }
    }
}
