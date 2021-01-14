using RedisViewer.Core;
using RedisViewer.UI.ViewModels;
using System.Windows.Controls;

namespace RedisViewer.UI.Views
{
    sealed partial class ServerInfoView : UserControl
    {
        public ServerInfoView()
        {
            InitializeComponent();
            DataContext = UnityResolver.Resolve<IServerInfoViewModel>();
        }
    }
}