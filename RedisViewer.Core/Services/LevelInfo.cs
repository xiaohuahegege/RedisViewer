using StackExchange.Redis;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace RedisViewer.Core
{
    public class LevelInfo : InfoBase
    {
        private readonly IServer _server;
        private readonly IDatabase _database;

        public LevelInfo(IServer server, IDatabase database, string tag, string name, int level)
        {
            _database = database;
            _server = server;

            Tag = tag;
            Name = name;
            Level = 2 + level;
        }

        public async Task ReloadAsync()
        {

        }

        public async Task<bool> DeleteAsync()
        {
            var keys = new List<RedisKey>();

            await foreach (var key in _server.KeysAsync(_database.Database, $"{Tag}:*"))
                keys.Add(key);

            return (await _database.KeyDeleteAsync(keys.ToArray())) == keys.Count;
        }

        private string _tag;
        public string Tag
        {
            get => _tag;
            set => SetProperty(ref _tag, value);
        }

        public ObservableCollection<object> _keys;
        public ObservableCollection<object> Keys
        {
            get => _keys;
            set => SetProperty(ref _keys, value);
        }
    }
}