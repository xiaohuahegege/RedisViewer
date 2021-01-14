using Prism.Mvvm;
using Prism.Regions;

namespace RedisViewer.UI.ViewModels
{
    internal class ServerInfoViewModel : ViewModelBase, INavigationAware, IServerInfoViewModel
    {
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
    }
}