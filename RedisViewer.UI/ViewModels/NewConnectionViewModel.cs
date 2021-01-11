using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using RedisViewer.Core;
using System.Threading.Tasks;

namespace RedisViewer.UI.ViewModels
{
    internal class NewConnectionViewModel : DialogViewModelBase, INewConnectionViewModel
    {
        /// <summary>
        /// Test command
        /// </summary>
        public DelegateCommand TestCommand { get; }

        /// <summary>
        /// Save command
        /// </summary>
        public DelegateCommand OkCommand { get; }

        /// <summary>
        /// Cancel command
        /// </summary>
        public DelegateCommand CancelCommand { get; }

        private readonly IMessageService _messageService;
        private readonly IConnectionService _connectionService;

        public NewConnectionViewModel(IMessageService messageService, IConnectionService connectionService)
        {
            _messageService = messageService;
            _connectionService = connectionService;

            // Test command
            TestCommand = new DelegateCommand(() =>
            {
                Task.Run(async () =>
                {
                    var success = await Connection.TestConnectAsync();
                    DispatcherService.BeginInvoke(() => _messageService.Show("Redis Viewer", success ? "Successful connection to redis-server" : "Can't connect to redis-server"));
                });
            });

            // Save command
            OkCommand = new DelegateCommand(() =>
            {
                Task.Run(async () =>
                {
                    if (await _connectionService.AddAsync(Connection)) // If added success
                        DispatcherService.BeginInvoke(() => CloseDialog(ButtonResult.OK, new DialogParameters { { "connection_info", Connection } }));
                    else
                        DispatcherService.BeginInvoke(() => _messageService.Show("Redis Viewer", "Add connection profile failed"));
                });
            });

            // Cancel command
            CancelCommand = new DelegateCommand(() => CloseDialog(ButtonResult.Cancel));
        }

        private ConnectionInfo _connection = new ConnectionInfo();
        public ConnectionInfo Connection
        {
            get => _connection;
            set => SetProperty(ref _connection, value);
        }
    }
}