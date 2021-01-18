using Prism.Mvvm;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        private ObservableCollection<object> _keys;
        public ObservableCollection<object> Keys
        {
            get => _keys;
            set => SetProperty(ref _keys, value);
        }

        private readonly IServer _server;
        private readonly IDatabase _database;

        public DatabaseInfo(IServer server, IDatabase database, long size)
        {
            _server = server;
            _database = database;
            _index = _database.Database;
            _size = size;

            Level = 1;
        }

        public async Task LoadAsync()
        {
            if (!IsInitialized)
            {
                IsInitialized = IsExpanded = true;

                if (Keys == null)
                    Keys = new ObservableCollection<object>();
                else
                    Keys.Clear();

                var size = 0;

                await foreach (var key in _server.KeysAsync(_database.Database))
                {
                    var name = key.ToString();

                    if (name.Contains(":"))
                    {
                        var names = key.ToString().Split(':');
                        var index = 0;

                        LevelInfo newLevel = null;

                        while (names.Length > index)
                        {
                            if (newLevel == null)
                            {
                                LevelInfo oldLevel = Keys.FirstOrDefault(c => c is LevelInfo level && level.Name == names[index]) as LevelInfo;

                                if (oldLevel != null)
                                {
                                    newLevel = oldLevel;
                                }
                                else
                                {
                                    newLevel = new LevelInfo(_server, _database, names[index], names[index], index);
                                    Keys.Add(newLevel);
                                }
                            }
                            else
                            {
                                if (names.Length == (index + 1))
                                {
                                    if (newLevel.Keys == null)
                                        newLevel.Keys = new ObservableCollection<object>();

                                    newLevel.Keys.Insert(0, new KeyInfo(_database, name, newLevel.Level + 1));
                                }
                                else
                                {
                                    if (newLevel.Keys == null)
                                    {
                                        var subLevel = new LevelInfo(_server, _database, string.Join(":", names.Take(index + 1).ToArray()), names[index], index);

                                        newLevel.Keys = new ObservableCollection<object>();
                                        newLevel.Keys.Add(subLevel);
                                        newLevel = subLevel;
                                    }
                                    else
                                    {
                                        var level = newLevel.Keys.FirstOrDefault(c => c is LevelInfo level && level.Name == names[index]) as LevelInfo;

                                        if (level != null)
                                        {
                                            newLevel = level;
                                        }
                                        else
                                        {
                                            var subLevel = new LevelInfo(_server, _database, string.Join(":", names.Take(index + 1).ToArray()), names[index], index);

                                            if (newLevel.Keys == null)
                                                newLevel.Keys = new ObservableCollection<object>();

                                            newLevel.Keys.Add(subLevel);
                                            newLevel = subLevel;
                                        }
                                    }
                                }
                            }

                            index++;
                        }
                    }
                    else
                    {
                        Keys.Add(new KeyInfo(_database, name));
                    }

                    size++;
                }

                Size = size;
            }
        }

        public async Task ReloadAsync()
        {
            IsInitialized = false;
            await LoadAsync();
        }

        public bool RemoveKey(KeyInfo key)
        {
            return false;
            //var success = Keys?.Remove(Keys.FirstOrDefault(c => c.Name.Equals(key.Name))) ?? false;
            //Size = Keys?.Count ?? 0; // recalculate keys size

            //return success;
        }

        public async Task<bool> AddStringKeyAsync(string name, string value)
        {
            return await _database.StringSetAsync(name, value);
        }

        public async Task<bool> AddListKeyAsync(string name, string value)
        {
            return await _database.ListLeftPushAsync(name, value) > 0;
        }

        public async Task<bool> AddSetKeyAsync(string name, string value)
        {
            return await _database.SetAddAsync(name, value);
        }

        public async Task<bool> AddZsetKeyAsync(string name, string value, double score)
        {
            return await _database.SortedSetAddAsync(name, value, score);
        }

        public async Task<bool> AddHashKeyAsync(string name, string value, string key)
        {
            return await _database.HashSetAsync(name, key, value);
        }

        //public async Task<RedisValue> AddStreamKeyAsync(string name, string value, string id)
        //{
        //    var names = new List<NameValueEntry>();
        //    names.Add(new NameValueEntry("", value));

        //    // return await _database.StreamAddAsync(name, names.ToArray());

        //    await _database.StreamAdd(name, null, value);
        //    //return await _database.StreamAddAsync(name, id, value);
        //}
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