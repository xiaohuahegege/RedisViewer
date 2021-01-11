using RedisViewer.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace RedisViewer.UI.Views.Components
{
    sealed partial class ValueEditor : UserControl
    {
        public ValueEditor()
        {

            InitializeComponent();
        }

        public static readonly DependencyProperty FieldProperty =
            DependencyProperty.Register("Field", typeof(string), typeof(ValueEditor));

        public object Field
        {
            get => GetValue(FieldProperty);
            set => SetValue(FieldProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(ValueEditor));

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
    }
}