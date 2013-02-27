using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Sharomank.RegexTester.Common;

namespace Sharomank.RegexTester.Handlers
{
    /// <summary>
    /// Author: Roman Kurbangaliev (Sharomank)
    /// </summary>
    public class RegexProcessHandler
    {
        public static void StartMode(BackgroundWorker worker, RegexTesterPageViewModel viewModel, DoWorkEventArgs args)
        {
            viewModel.PrepareProcess();
            RegexProcessContext context = (RegexProcessContext)args.Argument;
            bool complete = true;

            if (context.MatchRegex.IsMatch(context.InputText))
            {
                if (context.CurrentMode == RegexMode.Match)
                    complete = Match(worker, viewModel, context);
                else if (context.CurrentMode == RegexMode.Replace)
                    complete = Replace(worker, viewModel, context);
                else
                    complete = Split(worker, viewModel, context);
            }

            if (!complete)
            {
                args.Cancel = true;
            }
        }

        private static bool Match(BackgroundWorker worker, RegexTesterPageViewModel viewModel, RegexProcessContext context)
        {
            var matches = context.MatchRegex.Matches(context.InputText);

            int count = 0;
            var isSimpleMatch = string.IsNullOrEmpty(context.ReplaceRegexPattern);

            Dictionary<String, String> groups = GetMatchGroups(context, isSimpleMatch);

            foreach (Match item in matches)
            {
                if (worker.CancellationPending)
                    return false;

                if (item.Success)
                {
                    if (isSimpleMatch)
                    {
                        viewModel.AppendOutputText(item.Value);
                    }
                    else
                    {
                        string result = context.ReplaceRegexPattern;
                        foreach (var group in groups)
                        {
                            result = result.Replace(group.Value, item.Groups[group.Key].Value);
                        }
                        viewModel.AppendOutputText(result);
                    }
                    count++;
                }
            }
            viewModel.Count = count;
            return OperationIsComplete(worker);
        }

        private static Dictionary<String, String> GetMatchGroups(RegexProcessContext context, bool isSimpleMatch)
        {
            Dictionary<String, String> groups = new Dictionary<String, String>();

            if (isSimpleMatch)
                return groups;

            foreach (var groupName in context.MatchRegex.GetGroupNames())
            {
                string group = string.Format("$[{0}]", groupName);
                if (context.ReplaceRegexPattern.Contains(group))
                {
                    groups.Add(groupName, group);
                }
            }
            return groups;
        }

        private static bool Split(BackgroundWorker worker, RegexTesterPageViewModel viewModel, RegexProcessContext context)
        {
            int count = 0;
            var matches = context.MatchRegex.Split(context.InputText);
            foreach (var str in matches)
            {
                if (worker.CancellationPending)
                    return false;

                if (!string.IsNullOrEmpty(str))
                {
                    viewModel.AppendOutputText(str);
                    count++;
                }
            }
            viewModel.Count = count;
            return OperationIsComplete(worker);
        }

        private static bool Replace(BackgroundWorker worker, RegexTesterPageViewModel viewModel, RegexProcessContext context)
        {
            int count = 0;
            string result = context.MatchRegex.Replace(context.InputText, delegate(Match m)
            {
                string value = context.ReplaceRegexPattern;
                foreach (var groupName in context.MatchRegex.GetGroupNames())
                {
                    if (worker.CancellationPending)
                        return string.Empty;

                    value = value.Replace(string.Format("$[{0}]", groupName), m.Groups[groupName].Value);
                }
                count++;
                return value;
            });
            viewModel.Count = count;
            viewModel.AppendOutputText(result);
            return OperationIsComplete(worker);
        }

        private static bool OperationIsComplete(BackgroundWorker worker)
        {
            if (worker.CancellationPending)
                return false;
            return true;
        }
    }
}
