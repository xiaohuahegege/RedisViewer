using Prism.Mvvm;
using StackExchange.Redis;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RedisViewer.Core
{
    /// <summary>
    /// Database info
    /// </summary>
    public class DatabaseInfo : BindableBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        private KeyCollection _keys;
        public KeyCollection Keys
        {
            get => _keys;
            set => SetProperty(ref _keys, value);
        }

        private readonly IServer _server;
        private readonly IDatabase _database;
        public bool IsInitialized { get; private set; }

        public DatabaseInfo(IServer server, IDatabase database, long size)
        {
            _server = server;
            _database = database;
            _name = $"db{_database.Database} ({size})";
        }

        public async Task LoadKeyAsync()
        {
            try
            {
                if (!IsInitialized)
                {
                    Keys = new KeyCollection(_server, _database);
                    await Keys.LoadAsync();

                    IsExpanded = true;
                    IsInitialized = true;
                }
            }
            catch
            {
                IsInitialized = false;
            }
        }
    }

    public class DatabaseCollection : ObservableCollection<DatabaseInfo>
    {
        private readonly ConnectionMultiplexer _connection;

        public DatabaseCollection(ConnectionMultiplexer connection)
        {
            _connection = connection;
        }

        public void Load()
        {
            var server = _connection.GetServer(_connection.GetEndPoints()[0]);

            for (var i = 0; i < server.DatabaseCount; i++)
            {
                Add(new DatabaseInfo(server, _connection.GetDatabase(i), server.DatabaseSize(i)));
            }
        }
    }
}