using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharomank.RegexTester.Commands
{
    /// <summary>
    /// Author: Roman Kurbangaliyev (sharomank)
    /// </summary>
    public sealed class CommandEnum
    {
        private readonly String name;
        private readonly ICommand command;
        
        private static readonly Dictionary<string, CommandEnum> COMMANDS = new Dictionary<string, CommandEnum>();

        public static readonly CommandEnum NONE = new CommandEnum("", new NoneCommand());
        public static readonly CommandEnum SAVE = new CommandEnum("command-save", new SaveCommand());

        private CommandEnum(string name, ICommand command)
        {
            this.name = name;
            this.command = command;
            COMMANDS[name] = this;
        }

        public ICommand Command
        {
            get
            {
                return command;
            }
        }

        public String Name
        {
            get
            {
                return name;
            }
        }

        public override String ToString()
        {
            return name;
        }

        public static explicit operator CommandEnum(string commandName)
        {
            if (!COMMANDS.ContainsKey(commandName))
            {
                return NONE;
            }
            return COMMANDS.First(r => r.Key.Equals(commandName)).Value;
        }
    }
}
