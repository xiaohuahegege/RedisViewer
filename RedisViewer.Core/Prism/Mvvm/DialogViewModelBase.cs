using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using RedisViewer.Core;
using System;

namespace Prism.Mvvm
{
    public class DialogViewModelBase : ViewModelBase, IDialogAware
    {
        //private DelegateCommand<string> _closeDialogCommand;
        //public DelegateCommand<string> CloseDialogCommand =>
        //    _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public event Action<IDialogResult> RequestClose;

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected virtual void CloseDialog(ButtonResult result = ButtonResult.OK, IDialogParameters parameters = null)
        {
            RequestClose?.Invoke(new DialogResult(result, parameters));
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {

        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public DialogViewModelBase()
        {
            ScopedRegionManager = UnityResolver.Resolve<IRegionManager>().CreateRegionManager();
        }

        private IRegionManager _scopedRegionManager;

        public IRegionManager ScopedRegionManager
        {
            get => _scopedRegionManager;
            set => SetProperty(ref _scopedRegionManager, value);
        }
    }
}