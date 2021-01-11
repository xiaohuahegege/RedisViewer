using System.Windows;

namespace RedisViewer.UI.Controls
{
    public class ListBox : System.Windows.Controls.ListBox
    {
        static ListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListBox), new FrameworkPropertyMetadata(typeof(ListBox)));
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(ListBox));

        public DataTemplate HeaderTemplate
        {
            get => (DataTemplate)GetValue(HeaderTemplateProperty);
            set => SetValue(HeaderTemplateProperty, value);
        }

        public static readonly DependencyProperty FooterTemplateProperty =
            DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(ListBox));

        public DataTemplate FooterTemplate
        {
            get => (DataTemplate)GetValue(FooterTemplateProperty);
            set => SetValue(FooterTemplateProperty, value);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ListBoxItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ListBoxItem;
        }
    }
}