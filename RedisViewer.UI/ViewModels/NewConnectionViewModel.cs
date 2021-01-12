using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using RedisViewer.Core;
using RedisViewer.UI.Validators;
using System;
using System.Threading.Tasks;

namespace RedisViewer.UI.ViewModels
{
    internal class NewConnectionViewModel : ValidatableBindableBase<NewConnectionViewModel>, INewConnectionViewModel
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

        private readonly IMessageService<NewConnectionViewModel> _messageService;
        private readonly IConnectionService _connectionService;

        public NewConnectionViewModel(IMessageService<NewConnectionViewModel> messageService, IConnectionService connectionService)
        {
            _messageService = messageService;
            _connectionService = connectionService;

            SetValidator(new NewConnectionViewModelValidator(), this);

            // Test command
            TestCommand = new DelegateCommand(() =>
            {
                Task.Run(async () =>
                {
                    // show loading
                    DispatcherService.BeginInvoke(() => ShowLoading(true));

                    var success = await GetConnection().TestConnectAsync();

                    DispatcherService.BeginInvoke(() =>
                    {
                        ShowLoading(false);
                        _messageService.ShowAlert("Redis Viewer", success ? "Successful connection to redis-server" : "Can't connect to redis-server");
                    });
                });
            });

            // Save command
            OkCommand = new DelegateCommand(() =>
            {
                if (Validate())
                {
                    Task.Run(async () =>
                    {
                        // show loading
                        DispatcherService.BeginInvoke(() => ShowLoading(true));

                        var connection = GetConnection();
                        var success = await _connectionService.AddAsync(connection);

                        DispatcherService.BeginInvoke(() =>
                        {
                            ShowLoading(false);

                            if (success)
                                CloseDialog(ButtonResult.OK, new DialogParameters { { "connection_info", connection } });
                            else
                                _messageService.ShowAlert("Redis Viewer", "Add new connection failed");
                        });
                    });
                }
            });

            // Cancel command
            CancelCommand = new DelegateCommand(() => CloseDialog(ButtonResult.Cancel));
        }

        private ConnectionInfo GetConnection()
        {
            return new ConnectionInfo { Name = Name?.Trim(), Host = Host?.Trim(), Port = Convert.ToInt32(Port), Auth = Auth?.Trim() };
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _host = Constants.Default_Redis_Host;
        public string Host
        {
            get => _host;
            set
            {
                if (_host != value)
                    _host = value.Trim().Length <= 0 ? Constants.Default_Redis_Host : value;
            }
        }

        private string _port = Constants.Default_Redis_Port;
        public string Port
        {
            get => _port;
            set
            {
                if (_port != value)
                    _port = value.Trim().Length <= 0 ? Constants.Default_Redis_Port : value;
            }
        }

        private string _auth;
        public string Auth
        {
            get => _auth;
            set => SetProperty(ref _auth, value);
        }
    }
}