using Prism.Mvvm;
using StackExchange.Redis;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RedisViewer.Core
{
    /// <summary>
    /// Database info
    /// </summary>
    public class DatabaseInfo : InfoBase
    {
        private int _index;
        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        private long _size;
        public long Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
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
            _index = _database.Database;
            _size = size;
        }

        public async Task LoadAsync()
        {
            try
            {
                if (!IsInitialized)
                {
                    if (Keys == null)
                        Keys = new KeyCollection(_server, _database);

                    Keys.Clear();
                    await Keys.LoadAsync();

                    IsExpanded = true;
                    IsInitialized = true;
                    Size = Keys.Count; // set size
                }
            }
            catch
            {
                IsInitialized = false;
            }
        }

        public async Task ReloadAsync()
        {
            IsInitialized = false;
            await LoadAsync();
        }

        public bool RemoveKey(KeyInfo key)
        {
            var success = Keys?.Remove(Keys.FirstOrDefault(c => c.Name.Equals(key.Name))) ?? false;
            Size = Keys?.Count ?? 0; // recalculate keys size

            return success;
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