using Sharomank.RegexTester.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharomank.RegexTester.Strategies
{
    /// <summary>
    /// Author: Roman Kurbangaliyev (sharomank)
    /// </summary>
    public interface IRegexProcessStrategy
    {
        bool Match(BackgroundWorker worker, RegexTesterPageViewModel viewModel, RegexProcessContext context);
        bool Split(BackgroundWorker worker, RegexTesterPageViewModel viewModel, RegexProcessContext context);
        bool Replace(BackgroundWorker worker, RegexTesterPageViewModel viewModel, RegexProcessContext context);
    }
}
