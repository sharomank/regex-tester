using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sharomank.RegexTester.Commands
{
    /// <summary>
    /// Author: Roman Kurbangaliyev (sharomank)
    /// </summary>
    public class NoneCommand : ICommand
    {
        public void Execute(CommandContext ctx)
        {
            MessageBox.Show("Sorry, I can't find implementation for selected action.");
        }
    }
}
