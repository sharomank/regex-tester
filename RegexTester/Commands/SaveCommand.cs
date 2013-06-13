using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sharomank.RegexTester.Commands
{
    /// <summary>
    /// Author: Roman Kurbangaliyev (sharomank)
    /// </summary>
    public class SaveCommand : ICommand
    {
        private static string PREFIX = new String('-', 5);
        private static string POSTFIX = new String('-', 35);

        public void Execute(CommandContext ctx)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            string defaultFileName = string.Format("RegexTester_{0}", DateTime.Now.ToString("yyyy_MM_dd__HH_mm_ss"));
            dlg.FileName = defaultFileName;
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string fileName = dlg.FileName;
                StringBuilder sb = new StringBuilder();
                sb.Append("Regex Tester: version=").AppendLine(GetVersion())
                  .AppendLine(new String('-', 40))
                  .Append("REGEX MODE: ").AppendLine(ctx.RegexMode.ToString())
                  .Append("REGEX OPTIONS: ").AppendLine(ctx.RegexOptions.ToString())
                  .Append("INPUT REGEX: ").AppendLine(ctx.InputRegex)
                  .Append("INPUT FORMAT: ").AppendLine(ctx.InputFormat)
                  .AppendLine(string.Format("{0}INPUT TEXT{1}", PREFIX, POSTFIX))
                  .AppendLine(ctx.InputText)
                  .AppendLine(string.Format("{0}OUTPUT TEXT{1}", PREFIX, POSTFIX))
                  .Append(ctx.OutputText);
                File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);
            }
        }

        private String GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }
}
