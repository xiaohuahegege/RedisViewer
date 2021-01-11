using RedisViewer.Core;
using RedisViewer.UI.Controls;
using RedisViewer.UI.ViewModels;

namespace RedisViewer.UI.Views
{
    sealed partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
            DataContext = UnityResolver.Resolve<IShellViewModel>();
        }
    }
}