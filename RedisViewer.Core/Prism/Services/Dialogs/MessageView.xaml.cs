using RedisViewer.UI.Controls;

namespace Prism.Services.Dialogs
{
    sealed partial class MessageView : Window
    {
        public MessageView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string Message { get; set; }
    }
}