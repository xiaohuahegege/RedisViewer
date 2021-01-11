using System.Windows;

namespace RedisViewer.UI.Controls
{
    public class PaginationRadioButton : System.Windows.Controls.RadioButton
    {
        static PaginationRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PaginationRadioButton), new FrameworkPropertyMetadata(typeof(PaginationRadioButton)));
        }
    }
}