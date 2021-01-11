using Prism.Mvvm;
using Prism.Regions;
using RedisViewer.Core;

namespace RedisViewer.UI.ViewModels
{
    /// <summary>
    /// Key type view model base, all key type view model need inherit this class
    /// 
    /// </summary>
    internal class KeyTypeViewModelBase : ViewModelBase, INavigationAware
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
            if (navigationContext.Parameters.ContainsKey("key") &&
                   navigationContext.Parameters["key"] is KeyInfo key && key != null)
            {
                Key = key;
                PageLoad();
            }
        }

        public void ShowLoading()
        {

        }

        public virtual void PageLoad()
        {

        }

        public virtual void PageQuery(int pageIndex)
        {

        }

        private KeyInfo _key;
        public KeyInfo Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        private int _pageIndex = 1;
        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                if (_pageIndex != value)
                {
                    _pageIndex = value;
                    RaisePropertyChanged();

                    PageQuery(value);
                }
            }
        }

        private int _pageSize = 999;
        public int PageSize
        {
            get => _pageSize;
            set => SetProperty(ref _pageSize, value);
        }

        private long _pageCount;
        public long PageCount
        {
            get => _pageCount;
            set => SetProperty(ref _pageCount, value);
        }
    }
}