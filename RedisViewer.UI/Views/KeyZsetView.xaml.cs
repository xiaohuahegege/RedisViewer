using RedisViewer.Core;
using RedisViewer.UI.ViewModels;
using System.Windows.Controls;

namespace RedisViewer.UI.Views
{
    sealed partial class KeyZsetView : UserControl
    {
        public KeyZsetView()
        {
            InitializeComponent();
            DataContext = UnityResolver.Resolve<IKeyZsetViewModel>();
        }
    }
}