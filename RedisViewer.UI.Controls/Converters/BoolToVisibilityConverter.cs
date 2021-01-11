using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RedisViewer.UI.Controls.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool InvertVisibility { get; set; }
        public Visibility NotVisibleValue { get; set; }

        public BoolToVisibilityConverter()
        {
            InvertVisibility = false;
            NotVisibleValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Visible;

            bool visible = true;

            if (value is bool)
                visible = (bool)value;

            if (InvertVisibility)
                visible = !visible;

            return visible ? Visibility.Visible : this.NotVisibleValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((value is Visibility) && (((Visibility)value) == Visibility.Visible))
                          ? !InvertVisibility : InvertVisibility;
        }
    }
}