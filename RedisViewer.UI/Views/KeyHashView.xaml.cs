using RedisViewer.Core;
using RedisViewer.UI.ViewModels;
using System.Windows.Controls;

namespace RedisViewer.UI.Views
{
    sealed partial class KeyHashView : UserControl
    {
        public KeyHashView()
        {
            InitializeComponent();
            DataContext = UnityResolver.Resolve<IKeyHashViewModel>();
        }
    }
}
