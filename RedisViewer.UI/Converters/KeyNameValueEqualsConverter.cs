using System;
using System.Globalization;
using System.Windows.Data;

namespace RedisViewer.UI.Converters
{
    public class KeyNameValueEqualsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values != null &&
                values.Length == 2 &&
                values[0]?.ToString().Trim().Length > 0 &&
                values[1]?.ToString().Trim().Length > 1 &&
                !values[0].Equals(values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}