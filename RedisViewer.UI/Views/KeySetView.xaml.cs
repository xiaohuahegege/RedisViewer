using RedisViewer.Core;
using RedisViewer.UI.ViewModels;
using System.Windows.Controls;

namespace RedisViewer.UI.Views
{
    sealed partial class KeySetView : UserControl
    {
        public KeySetView()
        {
            InitializeComponent();
            DataContext = UnityResolver.Resolve<IKeySetViewModel>();
        }
    }
}