using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using RedisViewer.Core;
using RedisViewer.UI.Events;
using StackExchange.Redis;

namespace RedisViewer.UI.ViewModels
{
    internal class KeyViewerViewModel : ViewModelBase, INavigationAware, IKeyViewerViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly IMessageService<KeyViewerViewModel> _messageService;
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Delete key command
        /// </summary>
        public DelegateCommand DeleteCommand { get; }

        /// <summary>
        /// Reload key value command
        /// </summary>
        public DelegateCommand ReloadCommand { get; }

        /// <summary>
        /// Rename key command
        /// </summary>
        public DelegateCommand RenameCommand { get; }

        /// <summary>
        /// Set ttl command
        /// </summary>
        public DelegateCommand SetTTLCommand { get; }

        public KeyViewerViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IMessageService<KeyViewerViewModel> messageService)
        {
            _regionManager = regionManager;
            _messageService = messageService;
            _eventAggregator = eventAggregator;

            // Delete key command
            DeleteCommand = new DelegateCommand(async () =>
            {
                if (_messageService.ShowConfirm("Redis Viewer", "Do you really want to delete this key ?") == ButtonResult.OK)
                {
                    if (await _key.DeleteAsync())
                        _eventAggregator.GetEvent<DeleteKeyEvent>().Publish(_key);
                }
            });

            // Reload key value command
            ReloadCommand = new DelegateCommand(async () =>
            {
                _key = await _key.LoadAsync();
                ShowViewer();
            });

            // Rename key command
            RenameCommand = new DelegateCommand(async () =>
            {
                var success = await _key.RenameAsync(Name.Trim());

                if (success)
                    _key.Name = NewName = Name.Trim();

                _messageService.ShowAlert("Redis Viewer", success ? "Update Success" : "Update failure");

            });

            // Set ttl command
            SetTTLCommand = new DelegateCommand(async () =>
            {
                var value = long.Parse(TTL.Trim());
                var success = await _key.SetTTLAsync(value);

                if (success)
                {
                    _key.TTL = value;
                    NewTTL = TTL.Trim();
                }

                _messageService.ShowAlert("Redis Viewer", success ? "TTL updated successfully" : "TTL update failed");

            });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Check the params from ShellView
            if (navigationContext.Parameters.ContainsKey("key") &&
                navigationContext.Parameters["key"] is KeyInfo key && key != null)
            {
                ShowLoading(true);

                _key = await key.LoadAsync(); // load key info, get the key ttl and type
                ShowViewer();
            }
        }

        private void ShowViewer()
        {
            Name = NewName = _key.Name;
            TTL = NewTTL = _key.TTL.ToString();
            Type = _key.Type;

            _regionManager.ShowKeyViewerByType(_key); // show key value detail by key type
        }

        private KeyInfo _key;

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _newName;
        public string NewName
        {
            get => _newName;
            set => SetProperty(ref _newName, value);
        }

        private string _ttl;
        public string TTL
        {
            get => _ttl;
            set => SetProperty(ref _ttl, value);
        }

        private string _newTTL;
        public string NewTTL
        {
            get => _newTTL;
            set => SetProperty(ref _newTTL, value);
        }

        private RedisType _type;
        public RedisType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
    }
}