using System.Windows;

namespace RedisViewer.UI.Controls
{
    public class Window : System.Windows.Window
    {
        public static readonly DependencyProperty NonClientAreaContentProperty =
            DependencyProperty.Register("NonClientAreaContent", typeof(object), typeof(Window), new PropertyMetadata(default(object)));

        public object NonClientAreaContent
        {
            get { return GetValue(NonClientAreaContentProperty); }
            set { SetValue(NonClientAreaContentProperty, value); }
        }

        public static readonly DependencyProperty IsShowLoadingProperty =
            DependencyProperty.Register("IsShowLoading", typeof(bool), typeof(Window), new PropertyMetadata(false));

        public bool IsShowLoading
        {
            get => (bool)GetValue(IsShowLoadingProperty);
            set => SetValue(IsShowLoadingProperty, value);
        }

        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
        }

        private System.Windows.Controls.Button _minimizeButton;
        private System.Windows.Controls.Button _maximixedButton;
        private System.Windows.Controls.Button _restoreButton;
        private System.Windows.Controls.Button _closeButton;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _minimizeButton = GetTemplateChild("minimizeButton") as System.Windows.Controls.Button;
            _maximixedButton = GetTemplateChild("maximixedButton") as System.Windows.Controls.Button;
            _restoreButton = GetTemplateChild("restoreButton") as System.Windows.Controls.Button;
            _closeButton = GetTemplateChild("closeButton") as System.Windows.Controls.Button;

            if (_minimizeButton != null && _maximixedButton != null && _restoreButton != null && _closeButton != null)
            {
                _minimizeButton.Click += (sender, e) => WindowState = WindowState.Minimized;
                _maximixedButton.Click += (sender, e) => WindowState = WindowState.Maximized;
                _restoreButton.Click += (sender, e) => WindowState = WindowState.Normal;
                _closeButton.Click += (sender, e) => Close();
            }
        }
    }
}