using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using RedisViewer.Core;
using RedisViewer.UI.Events;
using StackExchange.Redis;
using System;

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
        public DelegateCommand<string> RenameCommand { get; }

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
                if (await _key.DeleteAsync())
                    _eventAggregator.GetEvent<DeleteKeyEvent>().Publish(_key);
            });

            // Reload key value command
            ReloadCommand = new DelegateCommand(async () =>
            {
                _key = await _key.LoadAsync();

                Name = _key.Name;
                TTL = _key.TTL;

                ShowViewer();
            });

            // Rename key command
            RenameCommand = new DelegateCommand<string>(async (name) =>
            {
                var success = await _key.RenameAsync(name);

                if (success)
                    _key.Name = name;

                _messageService.ShowAlert("Redis Viewer", success ? "Name updated successfully" : "Name update failed");
            });

            // Set ttl command
            SetTTLCommand = new DelegateCommand(() =>
            {
                //await _key.SetTTLAsync(DateTime.Now);
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

                Name = _key.Name;
                TTL = _key.TTL;
                Type = _key.Type;

                ShowViewer();
            }
        }

        private void ShowViewer()
        {
            _regionManager.ShowKeyViewerByType(_key); // show key value detail by key type
        }

        private KeyInfo _key;

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _ttl;
        public string TTL
        {
            get => _ttl;
            set => SetProperty(ref _ttl, value);
        }

        private RedisType _type;
        public RedisType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
    }
}