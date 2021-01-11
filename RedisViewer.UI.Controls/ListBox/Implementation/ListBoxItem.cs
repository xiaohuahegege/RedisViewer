using System.Windows;

namespace RedisViewer.UI.Controls
{
    public class ListBoxItem : System.Windows.Controls.ListBoxItem
    {
        static ListBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(typeof(ListBoxItem)));
        }
    }
}