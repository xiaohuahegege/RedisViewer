using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RedisViewer.Core
{
    /// <summary>
    /// Connection info
    /// </summary>
    public class ConnectionInfo : InfoBase
    {
        private string _host;

        [JsonProperty("host", Required = Required.Always)]
        public string Host
        {
            get => _host;
            set => SetProperty(ref _host, value);
        }

        private int _port;

        [JsonProperty("port", Required = Required.Always)]
        public int Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        private string _auth;

        [JsonProperty("auth")]
        public string Auth
        {
            get => _auth;
            set => SetProperty(ref _auth, value);
        }

        private ConnectionSecurity _connectionSecurity = ConnectionSecurity.None;

        [JsonIgnore]
        public ConnectionSecurity ConnectionSecurity
        {
            get => _connectionSecurity;
            set => SetProperty(ref _connectionSecurity, value);
        }

        private bool _isConnecting;

        [JsonIgnore]
        public bool IsConnecting
        {
            get => _isConnecting;
            set => SetProperty(ref _isConnecting, value);
        }

        private bool _isConnected = false;

        [JsonIgnore]
        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

        private DatabaseCollection _databases;

        [JsonIgnore]
        public DatabaseCollection Databases
        {
            get => _databases;
            set => SetProperty(ref _databases, value);
        }

        private ConnectionMultiplexer _connection;

        public async Task<bool> ConnectAsync()
        {
            if (_connection != null && _connection.IsConnected)
                return true;

            IsConnecting = true;
            IsConnected = false;

            try
            {
                _connection = await ConnectionMultiplexer.ConnectAsync(GetConfigurationOptions());
                IsConnected = _connection?.IsConnected ?? false;
            }
            catch (Exception)
            {

            }

            IsConnecting = false;

            return IsConnected;
        }

        public async Task LoadAsync()
        {
            if (_connection != null && _connection.IsConnected)
            {
                if (Databases == null)
                    Databases = new DatabaseCollection(_connection);

                Databases.Clear();
                Databases.Load();

                IsExpanded = true;
            }
        }

        public async Task<bool> DisconnectAsync()
        {
            if (_connection != null && _connection.IsConnected)
                await _connection.CloseAsync();

            return true;
        }

        public void Clear()
        {
            Databases?.Clear();
        }

        public async Task<bool> TestConnectAsync()
        {
            try
            {
                var connection = await ConnectionMultiplexer.ConnectAsync(GetConfigurationOptions());

                if (connection.IsConnected)
                {
                    await connection.CloseAsync();
                    return true;
                }
            }
            catch (Exception)
            {

            }

            return false;
        }

        public ConfigurationOptions GetConfigurationOptions()
        {
            return new ConfigurationOptions
            {
                EndPoints = { { Host, Port } },
                Password = Auth,
                AllowAdmin = true
            };
        }
    }

    public class ConnectionCollection : ObservableCollection<ConnectionInfo>
    {
        public async Task<bool> RemoveAsync(ConnectionInfo connection)
        {
            return Remove(connection);
        }
    }

    public enum ConnectionSecurity
    {
        None,
        Ssl,
        SshTunnel
    }
}