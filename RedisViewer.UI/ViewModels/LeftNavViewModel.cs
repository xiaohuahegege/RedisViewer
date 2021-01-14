using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using RedisViewer.Core;
using RedisViewer.UI.Events;
using System.Linq;
using System.Windows;

namespace RedisViewer.UI.ViewModels
{
    internal class LeftNavViewModel : ViewModelBase, ILeftNavViewModel
    {
        public DelegateCommand PageLoadedCommand { get; }
        public DelegateCommand<InfoBase> NavCommand { get; }
        public DelegateCommand NewConnectionCommand { get; }
        public DelegateCommand<DatabaseInfo> NewKeyCommand { get; }
        public DelegateCommand<DatabaseInfo> ReloadKeysCommand { get; }
        public DelegateCommand<ConnectionInfo> ServerInfoCommand { get; }
        public DelegateCommand<ConnectionInfo> ReloadServersCommand { get; }
        public DelegateCommand<ConnectionInfo> DeleteConnectionCommand { get; }
        public DelegateCommand<KeyInfo> CopyKeyNameCommand { get; }
        public DelegateCommand<KeyInfo> DeleteKeyCommand { get; }

        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionDialogService _dialogService;
        private readonly IConnectionService _connectionService;

        public LeftNavViewModel(IRegionManager regionManager, IEventAggregator eventAggregator,
            IRegionDialogService dialogService, IConnectionService connectionService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _connectionService = connectionService;

            #region PageLoadedCommand

            PageLoadedCommand = new DelegateCommand(async () =>
            {
                var connection = await _connectionService.GetAllAsync();

                if (connection != null && connection.Count > 0)
                    DispatcherService.BeginInvoke(() => Connections = connection);
            });

            #endregion PageLoadedCommand

            #region NavCommand

            NavCommand = new DelegateCommand<InfoBase>(async (item) =>
            {
                if (item is ConnectionInfo connection)
                {
                    _selectedConnection = connection;

                    if (!connection.IsConnecting && !connection.IsConnected)
                    {
                        if (await connection.ConnectAsync())
                            await connection.LoadAsync();
                    }

                    return;
                }

                if (item is DatabaseInfo database)
                {
                    _selectedDatabase = database;

                    if (!database.IsInitialized)
                        await database.LoadAsync();

                    return;
                }

                if (item is KeyInfo key)
                {
                    _regionManager.ShowKeyViewer(key);
                    return;
                }
            });

            #endregion NavCommand

            #region NewConnectionCommand

            NewConnectionCommand = new DelegateCommand(() =>
            {
                _dialogService.ShowNewConnection(c =>
                {
                    if (c.Result == ButtonResult.OK
                        && c.Parameters.TryGetValue<ConnectionInfo>("connection_info", out var connection) && connection != null)
                    {
                        // Maybe Treeview is null
                        if (Connections == null)
                            Connections = new ConnectionCollection();

                        Connections.Add(connection);
                    }
                });
            });

            #endregion NewConnectionCommand

            #region NewKeyCommand

            NewKeyCommand = new DelegateCommand<DatabaseInfo>((database) =>
            {
                _dialogService.ShowNewKey(database, c =>
                {
                    if (c.Result == ButtonResult.OK)
                    {
                    }
                });
            });

            #endregion NewKeyCommand

            #region ReloadKeysCommand

            ReloadKeysCommand = new DelegateCommand<DatabaseInfo>(async (database) =>
            {
                await database.ReloadAsync();
            });

            #endregion ReloadKeysCommand

            #region ServerInfoCommand

            ServerInfoCommand = new DelegateCommand<ConnectionInfo>((connection) =>
            {
                _regionManager.ShowServerInfoView(connection);
            });

            #endregion ServerInfoCommand

            #region ReloadServersCommand

            ReloadServersCommand = new DelegateCommand<ConnectionInfo>(async (connection) =>
            {
                await connection.LoadAsync();
            });

            #endregion ReloadServersCommand

            DeleteConnectionCommand = new DelegateCommand<ConnectionInfo>(async (connection) =>
            {
                await connection.RemoveAsync();
                await _connectionService.RemoveAsync(connection);
                await Connections.RemoveAsync(connection);
            });

            #region CopyKeyNameCommand

            CopyKeyNameCommand = new DelegateCommand<KeyInfo>((key) =>
            {
                Clipboard.SetText(key.Name);
            });

            #endregion CopyKeyNameCommand

            DeleteKeyCommand = new DelegateCommand<KeyInfo>((key) => { });

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _eventAggregator.GetEvent<DeleteKeyEvent>().Subscribe(key =>
            {
                if (key != null && _selectedDatabase.Keys.Any(c => c.Name.Equals(key.Name)))
                    _selectedDatabase.RemoveKey(key);

                _regionManager.ShowHomeView();
            });
        }

        /*
         * level
         * 
         * -- Connection info
         * ---- Database info
         * ------ Key info
         * 
         */
        private ConnectionCollection _connections;
        public ConnectionCollection Connections
        {
            get => _connections;
            set => SetProperty(ref _connections, value);
        }

        private ConnectionInfo _selectedConnection;
        private DatabaseInfo _selectedDatabase;
    }
}