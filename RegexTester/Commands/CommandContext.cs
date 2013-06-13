using Sharomank.RegexTester.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sharomank.RegexTester.Commands
{
    /// <summary>
    /// Author: Roman Kurbangaliyev (sharomank)
    /// </summary>
    public class CommandContext
    {
        public RegexMode RegexMode { get; private set; }
        public RegexOptions RegexOptions { get; private set; }
        public string InputRegex { get; private set; }
        public string InputFormat { get; private set; }
        public string InputText { get; private set; }
        public string OutputText { get; private set; }

        public CommandContext(RegexMode mode, RegexOptions options, string inputRegex, string inputFormat, string inputText, string outputText)
        {
            this.RegexMode = mode;
            this.RegexOptions = options;
            this.InputRegex = inputRegex;
            this.InputFormat = inputFormat;
            this.InputText = inputText;
            this.OutputText = outputText;
        }
    }
}
