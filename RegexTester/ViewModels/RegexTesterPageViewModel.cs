using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;

namespace Sharomank.RegexTester.Common
{
    /// <summary>
    /// Author: Roman Kurbangaliyev (sharomank)
    /// </summary>
    public class RegexTesterPageViewModel : INotifyPropertyChanged
    {
        #region Fields

        private int OPTIMIZE_COUNT = 1000;

        private StringBuilder _outputText = new StringBuilder();
        private bool _optimize = true;
        private bool _autorun = true;
        private int _count = 0;
        private StatusInfo _statusInfo = StatusInfo.None;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public StatusInfo StatusInfo
        {
            get
            {
                return _statusInfo;
            }
            set
            {
                _statusInfo = value;
            }
        }

        public string Status
        {
            get
            {
                return _statusInfo.Title;
            }
        }

        public Brush StatusBrush
        {
            get
            {
                return _statusInfo.Brush;
            }
        }

        public bool Optimize
        {
            get
            {
                return _optimize;
            }
            set
            {
                _optimize = value;
                NotifyPropertyChanged("Optimize");
            }
        }
        
        public bool Autorun
        {
            get
            {
                return _autorun;
            }
            set
            {
                _autorun = value;
                NotifyPropertyChanged("Autorun");
            }
        }

        public String OutputText
        {
            get
            {
                if (Optimize && _outputText.Length > OPTIMIZE_COUNT)
                {
                    StatusInfo = StatusInfo.OptimizeIsWorked;
                    StringBuilder sb = new StringBuilder();
                    sb.Append(_outputText.ToString(0, OPTIMIZE_COUNT));
                    sb.Append(Environment.NewLine);
                    sb.Append(StatusInfo.Description);
                    return sb.ToString();
                }
                else
                {
                    StatusInfo = StatusInfo.None;
                    return _outputText.ToString(0, _outputText.Length);
                }
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }

        #endregion

        #region Methods

        public void PrepareProcess()
        {
            _outputText.Clear();
            _statusInfo = StatusInfo.None;
            _count = 0;
        }

        public void CompleteProcess()
        {
            NotifyPropertyChanged("Count");
            NotifyPropertyChanged("OutputText");
            NotifyPropertyChanged("Status");
            NotifyPropertyChanged("StatusTooltip");
            NotifyPropertyChanged("StatusBrush");
        }

        public void AppendOutputText(string text)
        {
            if (_outputText.Length == 0)
            {
                _outputText.Append(text);
            }
            else
            {
                _outputText.Append(Environment.NewLine).Append(text);
            }
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
