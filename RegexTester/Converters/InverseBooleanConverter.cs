using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using System.Globalization;

namespace Sharomank.RegexTester.Converters
{
    /// <summary>
    /// Author: Roman Kurbangaliyev (sharomank)
    /// </summary>
    public class InverseBooleanConverter : MarkupExtension, IValueConverter
    {
        private static InverseBooleanConverter _converter = null;

        public InverseBooleanConverter() {}

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
            {
                _converter = new InverseBooleanConverter();
            }
            return _converter;
        }
    }
}