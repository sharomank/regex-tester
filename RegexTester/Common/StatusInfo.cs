using System.Windows.Media;

namespace Sharomank.RegexTester.Common
{
    /// <summary>
    /// Author: Roman Kurbangaliev (Sharomank)
    /// </summary>
    public class StatusInfo
    {
        public static readonly StatusInfo None = new StatusInfo("", "", "", Brushes.Black);
        public static readonly StatusInfo OptimizationIsWorked = new StatusInfo(
            "Optimization is enabled.",
            "Optimization mode shows only first 1000 chars and add warning in the end of output data.",
            "*** To see all the output data turn off Optimization mode. ***",
            Brushes.Green);

        private string _title;
        private string _tooltip;
        private string _description;
        private Brush _brush;

        private StatusInfo(string title, string tooltip, string description, Brush brush)
        {
            _title = title;
            _tooltip = tooltip;
            _description = description;
            _brush = brush;
        }

        public string Title
        {
            get
            {
                return _title;
            }
        }

        public string Tooltip
        {
            get
            {
                return _tooltip;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public Brush Brush
        {
            get
            {
                return _brush;
            }
        }
    }
}
