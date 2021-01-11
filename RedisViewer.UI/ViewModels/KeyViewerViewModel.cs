using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using RedisViewer.Core;
using StackExchange.Redis;
using System;

namespace RedisViewer.UI.ViewModels
{
    internal class KeyViewerViewModel : ViewModelBase, INavigationAware, IKeyViewerViewModel
    {
        private readonly IRegionManager _regionManager;

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

        public KeyViewerViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            // Delete key command
            DeleteCommand = new DelegateCommand(async () =>
            {
                await _key.DeleteAsync();
            });

            // Reload key value command
            ReloadCommand = new DelegateCommand(() =>
            {
                ShowViewer();
            });

            // Rename key command
            RenameCommand = new DelegateCommand<string>(async (name) =>
            {
                if (await _key.RenameAsync(name))
                    _key.Name = name;
            });

            // Set ttl command
            SetTTLCommand = new DelegateCommand(async () =>
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

        private TimeSpan? _ttl;
        public TimeSpan? TTL
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