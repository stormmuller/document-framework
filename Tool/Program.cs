using System;
using System.Collections.Generic;

namespace DocumentFramework.Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = new string[args.Length];
            Array.Copy(args, arguments, args.Length);

            var commands = new List<ICommand>();

           commands.Add(new HelpCommand(commands));            

            var commandDispatcher = new CommandDispatcher(commands);

            while(arguments.Length > 0)
            {
                arguments = commandDispatcher.DispatchCommand(arguments);
            }
        }
    }
}
