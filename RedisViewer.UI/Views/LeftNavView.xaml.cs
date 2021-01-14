using RedisViewer.Core;
using RedisViewer.UI.ViewModels;
using System.Windows.Controls;

namespace RedisViewer.UI.Views
{
    sealed partial class LeftNavView : UserControl
    {
        public LeftNavView()
        {
            InitializeComponent();
            DataContext = UnityResolver.Resolve<ILeftNavViewModel>();
        }
    }
}