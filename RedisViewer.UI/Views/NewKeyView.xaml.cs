using Prism.Services.Dialogs;
using RedisViewer.Core;
using RedisViewer.UI.Controls;
using RedisViewer.UI.ViewModels;

namespace RedisViewer.UI.Views
{
    sealed partial class NewKeyView : Window, IDialogWindow
    {
        public NewKeyView()
        {
            InitializeComponent();
            DataContext = UnityResolver.Resolve<INewKeyViewModel>();
        }

        public IDialogResult Result { get; set; }
    }
}