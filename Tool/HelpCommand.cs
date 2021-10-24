using System;
using System.Collections.Generic;

namespace DocumentFramework.Tool
{
    internal class HelpCommand : ICommand
    {
        private readonly IList<ICommand> _commands;

        public string Text => "help";
        public string ShorthandText => "h";
        public string Description => "Show command line help.";

        public HelpCommand(IList<ICommand> commands)
        {
            _commands = new List<ICommand>(commands);
            _commands.Add(this);
        }

        public void HandleCommand(string[] args)
        {
            Console.WriteLine("Options:");

            foreach (var command in _commands)
            {
                Console.Write($"-{command.ShorthandText}, --{command.Text}".PadRight(40));
                Console.WriteLine(command.Description);
            }
        }
    }
}