using System.Windows.Input;

namespace RedisViewer.UI.Controls
{
    public static class PaginationCommands
    {
        public static RoutedCommand Prev { get; }
            = new RoutedCommand(nameof(Prev), typeof(PaginationCommands));

        public static RoutedCommand Next { get; }
            = new RoutedCommand(nameof(Next), typeof(PaginationCommands));

        public static RoutedCommand Selected { get; }
            = new RoutedCommand(nameof(Selected), typeof(PaginationCommands));
    }
}