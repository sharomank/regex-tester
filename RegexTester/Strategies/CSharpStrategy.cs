using Sharomank.RegexTester.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sharomank.RegexTester.Strategies
{
    /// <summary>
    /// Author: Roman Kurbangaliyev (sharomank)
    /// </summary>
    public class CSharpStrategy : IRegexProcessStrategy
    {
        public bool Match(BackgroundWorker worker, RegexTesterPageViewModel viewModel, RegexProcessContext context)
        {
            viewModel.AppendOutputText("string pattern = @\"" + StringFormat(context.MatchRegex.ToString()) + "\";");
            viewModel.AppendOutputText("RegexOptions regexOptions = " + GetRegexOptions(context.MatchRegex.Options) + ";");
            viewModel.AppendOutputText("Regex regex = new Regex(pattern, regexOptions);");
            viewModel.AppendOutputText("string inputData = @\"" + StringFormat(context.InputText) + "\";");
            viewModel.AppendOutputText("foreach (Match match in regex.Matches(inputData))");
            viewModel.AppendOutputText("{");
            viewModel.AppendOutputText("\tif (match.Success)");
            viewModel.AppendOutputText("\t{");
            viewModel.AppendOutputText("\t\t//TODO processing results");
            viewModel.AppendOutputText("\t}");
            viewModel.AppendOutputText("}");

            return OperationIsComplete(worker, viewModel);
        }

        public bool Split(BackgroundWorker worker, RegexTesterPageViewModel viewModel, RegexProcessContext context)
        {
            viewModel.AppendOutputText("string pattern = @\"" + StringFormat(context.MatchRegex.ToString()) + "\";");
            viewModel.AppendOutputText("RegexOptions regexOptions = " + GetRegexOptions(context.MatchRegex.Options) + ";");
            viewModel.AppendOutputText("Regex regex = new Regex(pattern, regexOptions);");
            viewModel.AppendOutputText("string inputData = @\"" + StringFormat(context.InputText) + "\";");
            viewModel.AppendOutputText("string[] result = regex.Split(inputData);");

            return OperationIsComplete(worker, viewModel);
        }

        public bool Replace(BackgroundWorker worker, RegexTesterPageViewModel viewModel, RegexProcessContext context)
        {
            viewModel.AppendOutputText("string pattern = @\"" + StringFormat(context.MatchRegex.ToString()) + "\";");
            viewModel.AppendOutputText("RegexOptions regexOptions = " + GetRegexOptions(context.MatchRegex.Options) + ";");
            viewModel.AppendOutputText("Regex regex = new Regex(pattern, regexOptions);");
            viewModel.AppendOutputText("string inputData = @\"" + StringFormat(context.InputText) + "\";");
            viewModel.AppendOutputText("string replacement = @\"" + StringFormat(context.ReplaceRegexPattern) + "\";");
            viewModel.AppendOutputText("string result = regex.Replace(inputData, replacement);");
            
            return OperationIsComplete(worker, viewModel);
        }

        private string GetRegexOptions(RegexOptions selectedOptions)
        {
            StringBuilder sb = new StringBuilder();

            var availableOptions = Enum.GetValues(typeof(RegexOptions)).Cast<Enum>();
            var options = availableOptions.Where(selectedOptions.HasFlag);
            foreach (RegexOptions option in options) {
                if (option == RegexOptions.None && options.Count() > 1)
                {
                    continue;
                }
                if (sb.Length > 0)
                {
                    sb.Append(" | ");
                }
                sb.Append("RegexOptions." + option.ToString());
            }
            return sb.ToString();
        }

        private string StringFormat(string str)
        {
            return (str != null) ? str.Replace("\"", "\"\"") : str;
        }

        private bool OperationIsComplete(BackgroundWorker worker, RegexTesterPageViewModel viewModel)
        {
            viewModel.Count = 0;
            return !worker.CancellationPending;
        }
    }
}
