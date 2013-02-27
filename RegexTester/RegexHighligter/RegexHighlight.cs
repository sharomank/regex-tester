using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;

namespace Sharomank.RegexTester.RegexHighligter
{
    public class RegexHighligter
    {
        public TextRange Process(string regexExpression)
        {
            ProcessContext ctx = new ProcessContext(regexExpression);
            GetToken(ctx);
            if (RegexType.End.Equals(ctx.Type))
            {
                return null;
            }
            ProcessGroup();
            if (!RegexType.End.Equals(ctx.Type))
            {
                throw new ApplicationException();
            }
            return new TextRange(new TextPointer(), new TextPointer());
        }

        private void ProcessGroup()
        {
            
        }

        private void ProcessExpression()
        {

        }

        private void GetToken(ProcessContext ctx)
        {
            ctx.Type = RegexType.None;
            ctx.Token = string.Empty;

            if (ctx.ExpressionIndex == ctx.Expression.Length)
            {
                ctx.Type = RegexType.End;
            }

        }

        private class ProcessContext
        {
            public int ExpressionIndex { get; set; }
            public string Expression { get; set; }
            public string Token { get; set; }
            public RegexType Type { get; set; }

            public ProcessContext(string expression)
            {
                this.ExpressionIndex = 0;
                this.Expression = expression;
            }
        }

        /*
        private void HighlightRegex()
        {
            IsHighlighteringRegexSyntax = true;

            TextRange documentRange = new TextRange(rtbInputRegex.Document.ContentStart, rtbInputRegex.Document.ContentEnd);
            documentRange.ClearAllProperties();

            var l_tags = new List<TagInfo>();
            var l_special = new List<TagInfo>();
            var l_braces = new List<BracketInfo>();

            var chars = documentRange.Text.ToArray();
            var charIndex = 0;
            var nextCharSkip = false;
            foreach (char ch in chars)
            {
                if (nextCharSkip)
                {
                    nextCharSkip = false;
                }
                else if (ch == '\\')
                {
                    var nextIndex = chars.Length >= charIndex + 1 ? charIndex + 1 : charIndex;

                    var _word = string.Format("\\{0}", chars.ElementAt(nextIndex));
                    if ((charIndex == 0 || chars.ElementAt(charIndex - 1) != '\\') &&
                        RegexSyntaxProvider.GetCharacterClasses.Contains(_word))
                    {
                        l_tags.Add(new TagInfo
                        {
                            StartPosition = documentRange.Start.GetPositionAtOffset(charIndex, LogicalDirection.Forward),
                            EndPosition = documentRange.Start.GetPositionAtOffset(charIndex + 2, LogicalDirection.Backward)
                        });
                        nextCharSkip = true;
                    }
                }
                else if (RegexSyntaxProvider.GetSpecialChars.Contains(ch))
                {
                    if (charIndex == 0 || chars.ElementAt(charIndex - 1) != '\\')
                    {
                        l_special.Add(new TagInfo
                        {
                            StartPosition = documentRange.Start.GetPositionAtOffset(charIndex, LogicalDirection.Forward),
                            EndPosition = documentRange.Start.GetPositionAtOffset(charIndex + 1, LogicalDirection.Backward)
                        });
                    }
                }
                else if (RegexSyntaxProvider.GetBeginBrackets.Contains(ch))
                {
                    if (charIndex == 0 || chars.ElementAt(charIndex - 1) != '\\')
                    {
                        l_braces.Add(new BracketInfo
                        {
                            BeginStartPosition = documentRange.Start.GetPositionAtOffset(charIndex, LogicalDirection.Forward),
                            BeginStopPosition = documentRange.Start.GetPositionAtOffset(charIndex + 1, LogicalDirection.Backward),
                            Type = RegexSyntaxProvider.GetBracketType(ch)
                        });
                    }
                }
                else if (RegexSyntaxProvider.GetEndBrackets.Contains(ch))
                {
                    if (charIndex == 0 || chars.ElementAt(charIndex - 1) != '\\')
                    {
                        var last = l_braces.Where(r => r.EndStartPosition == null && r.Type == RegexSyntaxProvider.GetBracketType(ch));
                        if (last.Count() > 0)
                        {
                            var lst = last.Last();
                            lst.EndStartPosition = documentRange.Start.GetPositionAtOffset(charIndex, LogicalDirection.Forward);
                            lst.EndStopPosition = documentRange.Start.GetPositionAtOffset(charIndex + 1, LogicalDirection.Backward);
                        }
                        else
                        {
                            l_braces.Add(new BracketInfo
                            {
                                EndStartPosition = documentRange.Start.GetPositionAtOffset(charIndex, LogicalDirection.Forward),
                                EndStopPosition = documentRange.Start.GetPositionAtOffset(charIndex + 1, LogicalDirection.Backward),
                                Type = RegexSyntaxProvider.GetBracketType(ch)
                            });
                        }
                    }
                }

                charIndex++;
            }

            Format(l_tags, l_special, l_braces);

            l_tags = null;
            l_special = null;
            l_braces = null;

            IsHighlighteringRegexSyntax = false;
        }

        private void Format(List<TagInfo> l_tags, List<TagInfo> l_special, List<BracketInfo> l_braces)
        {
            rtbInputRegex.TextChanged -= TextChangedEventHandler;

            var red = new SolidColorBrush(Colors.Red);
            var blue = new SolidColorBrush(Colors.Blue);
            var yellow = new SolidColorBrush(Colors.Yellow);
            var green = new SolidColorBrush(Color.FromRgb(135, 255, 120));
            var brown = new SolidColorBrush(Color.FromRgb(255, 205, 95));
            var violet = new SolidColorBrush(Colors.Violet);
            TextRange range = new TextRange(rtbInputRegex.Document.ContentStart, rtbInputRegex.Document.ContentEnd);

            for (int i = 0; i < l_tags.Count; i++)
            {
                range.Select(l_tags[i].StartPosition, l_tags[i].EndPosition);
                //range.ApplyPropertyValue(TextElement.BackgroundProperty, blue);
                //range.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }

            for (int i = 0; i < l_special.Count; i++)
            {
                range.Select(l_special[i].StartPosition, l_special[i].EndPosition);
                //range.ApplyPropertyValue(TextElement.BackgroundProperty, violet);
            }

            for (int i = 0; i < l_braces.Count; i++)
            {
                if (l_braces[i].BeginStopPosition == null)
                {
                    range.Select(l_braces[i].EndStartPosition, l_braces[i].EndStopPosition);
                    //range.ApplyPropertyValue(TextElement.BackgroundProperty, red);
                }
                else if (l_braces[i].EndStopPosition == null)
                {
                    range.Select(l_braces[i].BeginStartPosition, l_braces[i].BeginStopPosition);
                    //range.ApplyPropertyValue(TextElement.BackgroundProperty, red);
                }
                else if (l_braces[i].Type == RegexSyntaxProvider.BracketType.Square)
                {
                    range.Select(l_braces[i].BeginStartPosition, l_braces[i].EndStopPosition);
                    //range.ApplyPropertyValue(TextElement.BackgroundProperty, brown);
                }
                else
                {
                    range.Select(l_braces[i].BeginStartPosition, l_braces[i].BeginStopPosition);
                    //range.ApplyPropertyValue(TextElement.BackgroundProperty, l_braces[i].Type == RegexSyntaxProvider.BracketType.Brackets ? green : yellow);
                    range.Select(l_braces[i].EndStartPosition, l_braces[i].EndStopPosition);
                    //range.ApplyPropertyValue(TextElement.BackgroundProperty, l_braces[i].Type == RegexSyntaxProvider.BracketType.Brackets ? green : yellow);
                }
            }

            rtbInputRegex.TextChanged += TextChangedEventHandler;
        }*/
    }
}
