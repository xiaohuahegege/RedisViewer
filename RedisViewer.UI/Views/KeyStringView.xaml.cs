using RedisViewer.Core;
using RedisViewer.UI.ViewModels;
using System.Windows.Controls;

namespace RedisViewer.UI.Views
{
    sealed partial class KeyStringView : UserControl
    {
        public KeyStringView()
        {
            InitializeComponent();
            DataContext = UnityResolver.Resolve<IKeyStringViewModel>();
        }
    }
}