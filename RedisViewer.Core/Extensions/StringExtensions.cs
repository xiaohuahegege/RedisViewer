using System.Text.RegularExpressions;

namespace RedisViewer.Core
{
    public static class StringExtensions
    {
        public static bool IsPort(this string value)
            => value != null && Regex.IsMatch(value.Trim(), @"^[1-9]\d*$");
    }
}