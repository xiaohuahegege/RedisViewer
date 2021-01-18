using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using RedisViewer.Core;
using RedisViewer.UI.Events;
using System.Threading.Tasks;
using System.Windows;

namespace RedisViewer.UI.ViewModels
{
    internal partial class LeftNavViewModel : ViewModelBase, ILeftNavViewModel
    {
        public DelegateCommand PageLoadedCommand { get; }
        public DelegateCommand<InfoBase> NavCommand { get; }
        public DelegateCommand<ConnectionInfo> ServerInfoCommand { get; }
        public DelegateCommand<ConnectionInfo> DeleteConnectionCommand { get; }
        public DelegateCommand<ConnectionInfo> ReloadConnectionCommand { get; }
        public DelegateCommand NewConnectionCommand { get; }
        public DelegateCommand<DatabaseInfo> ReloadDatabaseCommand { get; }
        public DelegateCommand<InfoBase> NewKeyCommand { get; }
        public DelegateCommand<LevelInfo> DeleteLevelCommand { get; }
        public DelegateCommand<LevelInfo> CopyLevelNameCommand { get; }
        public DelegateCommand<LevelInfo> ReloadLevelCommand { get; }
        public DelegateCommand<KeyInfo> DeleteKeyCommand { get; }
        public DelegateCommand<KeyInfo> CopyKeyNameCommand { get; }

        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionDialogService _dialogService;
        private readonly IConnectionService _connectionService;
        private readonly IMessageService<LeftNavViewModel> _messageService;

        public LeftNavViewModel(IRegionManager regionManager, IEventAggregator eventAggregator,
            IRegionDialogService dialogService, IConnectionService connectionService,
            IMessageService<LeftNavViewModel> messageService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _connectionService = connectionService;
            _messageService = messageService;

            #region PageLoadedCommand

            PageLoadedCommand = new DelegateCommand(() =>
            {
                Task.Run(async () =>
                {
                    var connection = await _connectionService.GetAllAsync();

                    if (connection != null && connection.Count > 0)
                        DispatcherService.BeginInvoke(() => Connections = connection);
                });
            });

            #endregion PageLoadedCommand

            #region NavCommand

            NavCommand = new DelegateCommand<InfoBase>(async (info) =>
            {
                if (info != null)
                {
                    if (info is ConnectionInfo connection)
                    {
                        _selectedConnection = connection;

                        if (!connection.IsConnecting && !connection.IsConnected)
                        {
                            if (await connection.ConnectAsync())
                                await connection.LoadAsync();
                            else
                                _messageService.ShowAlert("Redis Viewer", $"Cannot connect to server '{connection.Name}'");
                        }
                    }
                    else if (info is DatabaseInfo database)
                    {
                        _selectedDatabase = database;

                        if (!database.IsInitialized)
                            await database.LoadAsync();
                    }
                    else if (info is LevelInfo level)
                    {
                        if (!level.IsInitialized)
                            level.IsExpanded = level.IsInitialized = true;
                    }
                    else if (info is KeyInfo key)
                    {
                        _regionManager.ShowKeyViewer(key);
                    }
                }
            });

            #endregion NavCommand

            #region ServerInfoCommand

            ServerInfoCommand = new DelegateCommand<ConnectionInfo>((connection) =>
            {
                if (connection != null)
                    _regionManager.ShowServerInfoView(connection);
            });

            #endregion ServerInfoCommand

            #region DeleteConnectionCommand

            DeleteConnectionCommand = new DelegateCommand<ConnectionInfo>(async (connection) =>
            {
                if (connection != null)
                {
                    if (_messageService.ShowConfirm("Redis Viewer", "Do you really want to delete connection ?") == ButtonResult.OK)
                    {
                        if (await Connections.RemoveAsync(connection))
                        {
                            connection.Clear();

                            _ = Task.Run(async () =>
                              {
                                  await connection.DisconnectAsync();
                                  await _connectionService.RemoveAsync(connection);
                              });
                        }
                    }
                }
            });

            #endregion DeleteConnectionCommand

            #region ReloadConnectionCommand

            ReloadConnectionCommand = new DelegateCommand<ConnectionInfo>(async (connection) =>
            {
                if (connection != null)
                {
                    _selectedConnection = connection;

                    if (connection.IsConnected)
                    {
                        await connection.LoadAsync();
                    }
                    else if (!connection.IsConnecting)
                    {
                        if (await connection.ConnectAsync())
                            await connection.LoadAsync();
                        else
                            _messageService.ShowAlert("Redis Viewer", $"Cannot connect to server '{connection.Name}'");
                    }
                }
            });

            #endregion ReloadConnectionCommand

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

            #region ReloadDatabaseCommand

            ReloadDatabaseCommand = new DelegateCommand<DatabaseInfo>(async (database) =>
            {
                await ReloadDatabaseAsync(database);
            });

            #endregion ReloadDatabaseCommand

            #region NewKeyCommand

            NewKeyCommand = new DelegateCommand<InfoBase>((info) =>
            {
                if (info is DatabaseInfo database)
                {
                    _dialogService.ShowNewKey(info, async c =>
                    {
                        if (c.Result == ButtonResult.OK)
                            await ReloadDatabaseAsync(database);
                    });
                }
            });

            #endregion NewKeyCommand

            #region DeleteLevelCommand

            DeleteLevelCommand = new DelegateCommand<LevelInfo>(async (level) =>
            {
                if (level != null)
                {
                    await level.DeleteAsync();
                }
            });

            #endregion DeleteLevelCommand

            #region CopyLevelNameCommand

            CopyLevelNameCommand = new DelegateCommand<LevelInfo>((level) =>
            {
                if (level != null)
                    Clipboard.SetText($"{level.Tag}:*");
            });

            #endregion CopyLevelNameCommand

            #region ReloadLevelCommand

            ReloadLevelCommand = new DelegateCommand<LevelInfo>((level) =>
            {

            });

            #endregion ReloadLevelCommand

            #region DeleteKeyCommand

            DeleteKeyCommand = new DelegateCommand<KeyInfo>(async (key) =>
            {
                if (key != null)
                {
                    if (_messageService.ShowConfirm("Redis Viewer", "Do you really want to delete this key ?") == ButtonResult.OK)
                    {
                        if (await key.DeleteAsync())
                            DeleteKey(key);
                    }
                }
            });

            #endregion DeleteKeyCommand

            #region CopyKeyNameCommand

            CopyKeyNameCommand = new DelegateCommand<KeyInfo>((key) =>
            {
                if (key != null)
                    Clipboard.SetText(key.Name);
            });

            #endregion CopyKeyNameCommand

            SubscribeEvents();
        }

        private void DeleteKey(KeyInfo key)
        {
            //if (key != null && _selectedDatabase.Keys.Any(c => c.Name.Equals(key.Name)))
            //    _selectedDatabase.RemoveKey(key);

            //_regionManager.ShowHomeView();
        }

        private async Task ReloadDatabaseAsync(DatabaseInfo database)
        {
            if (database != null)
                await database.ReloadAsync();
        }

        private void SubscribeEvents()
        {
            _eventAggregator.GetEvent<DeleteKeyEvent>().Subscribe(DeleteKey);
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