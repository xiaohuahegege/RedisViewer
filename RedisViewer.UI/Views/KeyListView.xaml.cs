using RedisViewer.Core;
using RedisViewer.UI.ViewModels;
using System.Windows.Controls;

namespace RedisViewer.UI.Views
{
    sealed partial class KeyListView : UserControl
    {
        public KeyListView()
        {
            InitializeComponent();
            DataContext = UnityResolver.Resolve<IKeyListViewModel>();
        }
    }
}
