using System;
using System.Globalization;
using System.Windows.Data;

namespace RedisViewer.UI.Converters
{
    public class KeyTtlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ttl = (TimeSpan?)value;
            return ttl.HasValue ? System.Convert.ToInt32(ttl.Value.TotalSeconds).ToString() : "-1";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}