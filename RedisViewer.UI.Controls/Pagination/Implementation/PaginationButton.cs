using System.Windows;

namespace RedisViewer.UI.Controls
{
    public class PaginationButton : System.Windows.Controls.Button
    {
        static PaginationButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PaginationButton), new FrameworkPropertyMetadata(typeof(PaginationButton)));
        }
    }
}