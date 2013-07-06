using System.Windows.Media;

namespace Sharomank.RegexTester.Common
{
    /// <summary>
    /// Author: Roman Kurbangaliyev (sharomank)
    /// </summary>
    public class StatusInfo
    {
        public static readonly StatusInfo None = new StatusInfo("", "", Brushes.Black);
        public static readonly StatusInfo OptimizeIsWorked = new StatusInfo(
            "OPTIMIZE is enabled.",
            "*** To see all the output data turn off mode of OPTIMIZE. ***",
            Brushes.Green);

        private string _title;
        private string _description;
        private Brush _brush;

        private StatusInfo(string title, string description, Brush brush)
        {
            _title = title;
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
