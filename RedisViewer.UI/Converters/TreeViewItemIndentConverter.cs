using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RedisViewer.UI.Converters
{
    public class TreeViewItemIndentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && int.TryParse(value.ToString(), out var indent) && indent > 0)
                return new Thickness(indent * 5, 5, 10, 5);

            return new Thickness(2, 5, 10, 5);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}