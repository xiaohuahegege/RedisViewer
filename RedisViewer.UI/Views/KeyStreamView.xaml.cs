using RedisViewer.Core;
using RedisViewer.UI.ViewModels;
using System.Windows.Controls;

namespace RedisViewer.UI.Views
{
    sealed partial class KeyStreamView : UserControl
    {
        public KeyStreamView()
        {
            InitializeComponent();
            DataContext = UnityResolver.Resolve<IKeyStreamViewModel>();
        }
    }
}