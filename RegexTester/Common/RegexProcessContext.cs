using System;
using System.Text.RegularExpressions;

namespace Sharomank.RegexTester.Common
{
    /// <summary>
    /// Author: Roman Kurbangaliyev (sharomank)
    /// </summary>
    public class RegexProcessContext
    {
        private Regex _matchRegex;

        public string MatchRegexExpression { get; private set; }
        public RegexOptions MatchRegexOptions { get; private set; }
        public string ReplaceRegexPattern { get; private set; }
        public string InputText { get; private set; }
        public RegexMode CurrentMode { get; private set; }
        public OutputMode OutputMode { get; private set; }

        public RegexProcessContext(string matchRegexExpression, RegexOptions matchRegexOptions, string replaceRegexPattern, string inputText, RegexMode currentMode, OutputMode outputMode)
        {
            MatchRegexExpression = matchRegexExpression;
            MatchRegexOptions = matchRegexOptions;
            InputText = inputText;
            CurrentMode = currentMode;
            OutputMode = outputMode;
            if (replaceRegexPattern == null)
            {
                replaceRegexPattern = String.Empty;
            }
            ReplaceRegexPattern = replaceRegexPattern.Replace("\\n", Environment.NewLine).Replace("\\t", "	");
        }

        public Regex MatchRegex
        {
            get
            {
                if (_matchRegex == null)
                {
                    _matchRegex = new Regex(MatchRegexExpression, MatchRegexOptions);
                }
                return _matchRegex;
            }
        }
    }
}