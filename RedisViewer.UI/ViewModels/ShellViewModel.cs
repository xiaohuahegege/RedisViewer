using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using RedisViewer.Core;

namespace RedisViewer.UI.ViewModels
{
    /// <summary>
    /// Shell view model
    /// </summary>
    internal class ShellViewModel : ViewModelBase, IShellViewModel
    {
        /// <summary>
        /// Page load command
        /// </summary>
        public DelegateCommand LoadCommand { get; }

        /// <summary>
        /// Treeview item selection command
        /// </summary>
        public DelegateCommand<object> NavCommand { get; }

        /// <summary>
        /// New key view command
        /// </summary>
        public DelegateCommand<DatabaseInfo> NewKeyCommand { get; }

        /// <summary>
        /// New connection view command
        /// </summary>
        public DelegateCommand NewConnectionCommand { get; }

        private readonly IRegionManager _regionManager;
        private readonly IRegionDialogService _dialogService;
        private readonly IConnectionService _connectionService;

        public ShellViewModel(IRegionManager regionManager, IRegionDialogService dialogService,
            IConnectionService connectionService)
        {
            _regionManager = regionManager;
            _dialogService = dialogService;
            _connectionService = connectionService;

            // Page load command
            LoadCommand = new DelegateCommand(async () =>
            {
                // Get all connections from json configuration file
                var connection = await _connectionService.GetAllAsync();

                if (connection != null && connection.Count > 0)
                    DispatcherService.BeginInvoke(() => Connections = connection);
            });

            // Treeview item selection command
            NavCommand = new DelegateCommand<object>(async (item) =>
            {
                // Connection
                if (item is ConnectionInfo connection)
                {
                    // If connection is un-connecting or disconnected, start connection
                    if (!connection.IsConnecting && !connection.IsConnected)
                        await connection.ConnectAsync();
                    return;
                }

                // Database
                if (item is DatabaseInfo database)
                {
                    if (!database.IsInitialized)
                        await database.LoadKeyAsync(); // load all keys
                    return;
                }

                // Key
                if (item is KeyInfo key)
                {
                    _regionManager.ShowKeyViewer(key); // show key detail view
                    return;
                }
            });

            // New key view command
            NewKeyCommand = new DelegateCommand<DatabaseInfo>((database) =>
            {
                // Show new key view
                _dialogService.ShowNewKey(database, c =>
                {
                    // If add new key success, append the KeyInfo to the Treeview, not need reload the Treeview
                    if (c.Result == ButtonResult.OK)
                    {

                    }
                });
            });

            // New connection view command
            NewConnectionCommand = new DelegateCommand(() =>
            {
                // Show new connection view
                _dialogService.ShowNewConnection(c =>
                {
                    // If add new connection success, append the ConnectionInfo to the Treeview, not need reload the Treeview
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
    }
}