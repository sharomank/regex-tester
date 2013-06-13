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
    public sealed class Commands
    {
        private readonly String name;
        private readonly ICommand command;
        
        private static readonly Dictionary<string, Commands> COMMANDS = new Dictionary<string, Commands>();

        public static readonly Commands NONE = new Commands("", new NoneCommand());
        public static readonly Commands SAVE = new Commands("command-save", new SaveCommand());
        public static readonly Commands GENERATE_CS = new Commands("command-generate-cs", new NoneCommand());
        public static readonly Commands GENERATE_VB = new Commands("command-generate-vb", new NoneCommand());

        private Commands(string name, ICommand command)
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

        public static explicit operator Commands(string commandName)
        {
            if (!COMMANDS.ContainsKey(commandName))
            {
                return NONE;
            }
            return COMMANDS.First(r => r.Key.Equals(commandName)).Value;
        }
    }
}
