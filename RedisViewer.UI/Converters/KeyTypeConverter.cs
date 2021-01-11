using StackExchange.Redis;
using System;
using System.Globalization;
using System.Windows.Data;

namespace RedisViewer.UI.Converters
{
    public class KeyTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RedisType)
            {
                return (RedisType)value switch
                {
                    RedisType.String => "String",
                    RedisType.Set => "Set",
                    RedisType.SortedSet => "Zset",
                    RedisType.List => "List",
                    RedisType.Hash => "Hash",
                    RedisType.Stream => "Stream",
                    RedisType.Unknown => "Unknown",
                    _ => "None"
                };
            }

            return "None";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}