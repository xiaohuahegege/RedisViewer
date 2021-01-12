using Prism.Navigation;

namespace Prism.Mvvm
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        protected ViewModelBase()
        {

        }

        public virtual void Destroy()
        {

        }

        public void ShowLoading(bool isShow)
        {
            IsShowLoading = isShow;
        }

        private bool _isShowLoading;
        public bool IsShowLoading
        {
            get => _isShowLoading;
            set => SetProperty(ref _isShowLoading, value);
        }
    }
}