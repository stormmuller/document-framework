using System.Collections.Generic;
using System.Linq;

namespace DocumentFramework.Tool
{
    internal class CommandDispatcher
    {
        IList<ICommand> _commands;


        internal CommandDispatcher(IList<ICommand> commands)
        {
            _commands = commands;
        }

        internal string[] DispatchCommand(string[] args)
        {
            var commandToLookFor = args.First().Trim('-').ToLower();

            var command = _commands.Single(
                command => command.Text == commandToLookFor || command.ShorthandText == commandToLookFor
            );

            command.HandleCommand(args);

            return args.Skip(1).ToArray();
        }
    }
}