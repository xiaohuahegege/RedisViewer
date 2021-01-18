using RedisViewer.UI.Controls;

namespace Prism.Services.Dialogs
{
    sealed partial class AlertView : Window
    {
        public AlertView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string Message { get; set; }
    }
}