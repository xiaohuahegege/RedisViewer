using RedisViewer.Core;
using RedisViewer.UI.ViewModels;
using System.Windows.Controls;

namespace RedisViewer.UI.Views
{
    sealed partial class KeyViewerView : UserControl
    {
        public KeyViewerView()
        {
            InitializeComponent();
            DataContext = UnityResolver.Resolve<IKeyViewerViewModel>();
        }
    }
}