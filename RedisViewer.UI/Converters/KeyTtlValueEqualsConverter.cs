using System;
using System.Globalization;
using System.Windows.Data;

namespace RedisViewer.UI.Converters
{
    public class KeyTtlValueEqualsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null &&
                values.Length == 2)
            {
                var value0 = values[0]?.ToString().Trim();
                var value1 = values[1]?.ToString().Trim();

                if (value0 != null && value1 != null)
                {
                    if (value0.Contains(".") || value1.Contains("."))
                        return false;

                    if (long.TryParse(value0, out var value) && value <= 2147483647)
                        return !value0.Equals(value1);
                }
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}