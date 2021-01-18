using Prism.Commands;
using RedisViewer.UI.Controls;

namespace Prism.Services.Dialogs
{
    sealed partial class ConfirmView : Window
    {
        public DelegateCommand YesCommand { get; }
        public DelegateCommand NoCommand { get; }

        public ConfirmView()
        {
            InitializeComponent();

            YesCommand = new DelegateCommand(() => DialogResult = true);
            NoCommand = new DelegateCommand(() => DialogResult = false);

            DataContext = this;
        }

        public string Message { get; set; }
    }
}