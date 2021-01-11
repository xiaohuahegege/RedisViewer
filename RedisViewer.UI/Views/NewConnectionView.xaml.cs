using Prism.Services.Dialogs;
using RedisViewer.Core;
using RedisViewer.UI.Controls;
using RedisViewer.UI.ViewModels;

namespace RedisViewer.UI.Views
{
    sealed partial class NewConnectionView : Window, IDialogWindow
    {
        public NewConnectionView()
        {
            InitializeComponent();
            DataContext = UnityResolver.Resolve<INewConnectionViewModel>();
        }

        public IDialogResult Result { get; set; }
    }
}