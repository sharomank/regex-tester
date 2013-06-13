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
    public interface ICommand
    {
        void Execute(CommandContext ctx);
    }
}
