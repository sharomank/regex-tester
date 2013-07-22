using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Sharomank.RegexTester.Common;
using Sharomank.RegexTester.Handlers;
using Sharomank.RegexTester.Commands;
using System.Windows.Documents;

namespace Sharomank.RegexTester
{
    /// <summary>
    /// Author: Roman Kurbangaliyev (sharomank)
    /// </summary>
    public partial class RegexTesterPage : UserControl
    {
        #region Fields

        private static Brush REGEX_SYNTAX_ERROR_BRUSH = Brushes.Red;

        private BackgroundWorker worker = new BackgroundWorker();
        private RegexTesterPageViewModel viewModel = new RegexTesterPageViewModel();

        private bool initialize = false;
        private bool process_is_busy = false;

        #endregion

        public RegexTesterPage()
        {
            InitializeComponent();
            init();
            initialize = true;
        }

        private void init()
        {
            DataContext = viewModel;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;

            worker.DoWork += delegate(object s, DoWorkEventArgs args)
            {
                RegexProcessHandler.StartMode(worker, viewModel, args);
            };
            worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
            {
                if (!args.Cancelled && !worker.CancellationPending)
                {
                    viewModel.CompleteProcess();
                    RefreshHeaders();
                }
            };
        }

        #region Methods

        private RegexOptions GetRegexOptions()
        {
            var regexOptions = RegexOptions.None;
            if (IsSelectedRegexOption(cbCompiled))
                regexOptions = regexOptions | RegexOptions.Compiled;
            if (IsSelectedRegexOption(cbSingleline))
                regexOptions = regexOptions | RegexOptions.Singleline;
            if (IsSelectedRegexOption(cbMultiline))
                regexOptions = regexOptions | RegexOptions.Multiline;
            if (IsSelectedRegexOption(cbIgnoreCase))
                regexOptions = regexOptions | RegexOptions.IgnoreCase;
            if (IsSelectedRegexOption(cbIgnorePatternWhitespace))
                regexOptions = regexOptions | RegexOptions.IgnorePatternWhitespace;
            if (IsSelectedRegexOption(cbCultureInvariant))
                regexOptions = regexOptions | RegexOptions.CultureInvariant;
            if (IsSelectedRegexOption(cbECMAScript))
                regexOptions = regexOptions | RegexOptions.ECMAScript;
            if (IsSelectedRegexOption(cbExplicitCapture))
                regexOptions = regexOptions | RegexOptions.ExplicitCapture;
            if (IsSelectedRegexOption(cbRightToLeft))
                regexOptions = regexOptions | RegexOptions.RightToLeft;

            return regexOptions;
        }

        private bool IsSelectedRegexOption(CheckBox cb)
        {
            return cb.IsChecked == true && cb.IsEnabled == true;
        }

        private RegexMode GetCurrentMode()
        {
            if ((bool)rbtnReplace.IsChecked)
                return RegexMode.Replace;
            else if ((bool)rbtnSplitMode.IsChecked)
                return RegexMode.Split;
            else
                return RegexMode.Match;
        }

        private void ChangeRegexMode()
        {
            if (RegexRowReplace == null)
                return;

            RegexRowReplace.Height = (bool)rbtnSplitMode.IsChecked ? new GridLength(0) : new GridLength(1, GridUnitType.Star);
            tblockInputReplace.Visibility = (bool)rbtnSplitMode.IsChecked ? Visibility.Hidden : Visibility.Visible;
            rtbInputReplace.Visibility = (bool)rbtnSplitMode.IsChecked ? Visibility.Hidden : Visibility.Visible;

            AutorunProcess();
        }

        private void ControlKeyDown(KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
                return;

            if (e.Key == Key.P)//Ctrl + P
            {
                RunProcess();
                e.Handled = true;
            }
            else if (e.Key == Key.S)//Ctrl + S
            {
                ExecuteCommand(CommandEnum.SAVE);
                e.Handled = true;
            }

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
                return;

            if (e.Key == Key.A)//Ctrl + Shift + A
            {
                viewModel.Autorun = !viewModel.Autorun;
                e.Handled = true;
            }
        }

        private void RunProcess()
        {
            if (!initialize)
                return;

            string matchPattern = GetInputText(rtbInputRegex);

            #region Check matchPattern

            viewModel.PrepareProcess();

            if (string.IsNullOrEmpty(matchPattern))
            {
                ClearRegexSyntaxError(rtbInputRegex, rtbInputReplace);
                viewModel.CompleteProcess();
                ClearResultData();
                return;
            }

            try
            {
                Regex checkInputRegex = new Regex(matchPattern);
                ClearRegexSyntaxError(rtbInputRegex, rtbInputReplace);
            }
            catch (Exception ex)
            {
                AddRegexSyntaxError(rtbInputRegex, ex.Message);
                viewModel.CompleteProcess();
                ClearResultData();
                return;
            }

            #endregion

            if (worker.IsBusy)
            {
                worker.CancelAsync();
                int timeout = process_is_busy ? 500 : 100;
                System.Threading.Thread.Sleep(timeout);
            }

            RegexProcessContext context = new RegexProcessContext();
            context.MatchRegex = new Regex(matchPattern, GetRegexOptions());
            context.ReplaceRegexPattern = GetInputText(rtbInputReplace);
            context.CurrentMode = GetCurrentMode();
            context.InputText = tbInputText.Text;
            context.OutputMode = GetOutputMode();

            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync(context);
            }
            else
            {
                if (process_is_busy && viewModel.Autorun)
                {
                    process_is_busy = false;
                    viewModel.Autorun = false;
                }
                else
                {
                    process_is_busy = true;
                    RunProcess();
                }
            }
        }

        private OutputMode GetOutputMode()
        {
            return (OutputMode)Convert.ToInt32(((ComboBoxItem)cbOutputMode.SelectedItem).Tag);
        }

        private static string GetInputText(RichTextBox rtb)
        {
            TextRange documentRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            documentRange.ClearAllProperties();
            return documentRange.Text.Substring(0, documentRange.Text.Length - 2);
        }

        private static void AddRegexSyntaxError(RichTextBox rtb, String toolTip)
        {
            rtb.BorderThickness = new Thickness(2);
            rtb.BorderBrush = REGEX_SYNTAX_ERROR_BRUSH;
            rtb.ToolTip = toolTip;
        }

        private static void ClearRegexSyntaxError(RichTextBox rtb, RichTextBox rtbReplace)
        {
            if (REGEX_SYNTAX_ERROR_BRUSH.Equals(rtb.BorderBrush))
            {
                rtb.BorderThickness = rtbReplace.BorderThickness;
                rtb.BorderBrush = rtbReplace.BorderBrush;
                rtb.ToolTip = null;
            }
        }

        private void ClearResultData()
        {
            viewModel.PrepareProcess();
            viewModel.CompleteProcess();
            RefreshHeaders();
        }

        private void RefreshHeaders()
        {
            gbInput.Header = string.Format("Input text, length: {0}, lines: {1}", tbInputText.Text.Length, tbInputText.LineCount);
            gbOutput.Header = string.Format("Output text, length: {0}, lines: {1}", tbOutputText.Text.Length, tbOutputText.LineCount);
        }

        private void AutorunProcess()
        {
            if (viewModel.Autorun == true)
            {
                RunProcess();
            }
        }

        private void ExecuteCommand(CommandEnum command)
        {
            CommandContext ctx = CreateCommandContext();
            command.Command.Execute(ctx);
        }

        private CommandContext CreateCommandContext()
        {
            return new CommandContext(
                GetCurrentMode(),
                GetRegexOptions(),
                GetInputText(rtbInputRegex),
                GetInputText(rtbInputReplace),
                tbInputText.Text,
                tbOutputText.Text);
        }

        #endregion

        #region Events

        private void btnMatch_Click(object sender, RoutedEventArgs e)
        {
            RunProcess();
        }

        private void ModeItem_Checked(object sender, RoutedEventArgs e)
        {
            ChangeRegexMode();
        }

        private void TextChangedAutoRunEventHandler(object sender, TextChangedEventArgs e)
        {
            AutorunProcess();
        }

        private void CheckBoxAutoRun_Click(object sender, RoutedEventArgs e)
        {
            AutorunProcess();
        }

        private void RegexTesterControl_KeyDown(object sender, KeyEventArgs e)
        {
            ControlKeyDown(e);
        }

        private void cbAutoRun_Checked(object sender, RoutedEventArgs e)
        {
            if (tbSystemInfo != null)
                tbSystemInfo.Text = string.Empty;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            AutorunProcess();
        }

        private void cbOutputMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutorunProcess();
        }

        private void RegexTesterControl_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO load current mode
        }

        private void RegexTesterControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //TODO save current mode
        }

        private void btnCommand_Click(object sender, RoutedEventArgs e)
        {
            Button btnCommand = (Button)e.Source;
            if (btnCommand.Tag == null)
            {
                return;
            }
            string commandName = btnCommand.Tag.ToString();
            ExecuteCommand((CommandEnum)commandName);
        }

        #endregion
    }
}