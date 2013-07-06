using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Sharomank.RegexTester.Common;
using Sharomank.RegexTester.Strategies;

namespace Sharomank.RegexTester.Handlers
{
    /// <summary>
    /// Author: Roman Kurbangaliyev (sharomank)
    /// </summary>
    public class RegexProcessHandler
    {
        private static Dictionary<OutputMode, IRegexProcessStrategy> RegexProcessStrategy = new Dictionary<OutputMode, IRegexProcessStrategy>
        {
            {OutputMode.Default, new DefaultStrategy()},
            {OutputMode.CSharpSampleCode, new CSharpStrategy()},
        };

        public static void StartMode(BackgroundWorker worker, RegexTesterPageViewModel viewModel, DoWorkEventArgs args)
        {
            viewModel.PrepareProcess();
            RegexProcessContext context = (RegexProcessContext)args.Argument;
            bool complete = true;

            IRegexProcessStrategy strategy = RegexProcessStrategy[context.OutputMode];
            if (context.CurrentMode == RegexMode.Match)
            {
                complete = strategy.Match(worker, viewModel, context);
            }
            else if (context.CurrentMode == RegexMode.Replace)
            {
                complete = strategy.Replace(worker, viewModel, context);
            }
            else
            {
                complete = strategy.Split(worker, viewModel, context);
            }

            if (!complete)
            {
                args.Cancel = true;
            }
        }
    }
}
