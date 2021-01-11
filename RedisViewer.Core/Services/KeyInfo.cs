using Prism.Mvvm;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RedisViewer.Core
{
    /// <summary>
    /// Key info
    /// </summary>
    public class KeyInfo : BindableBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private RedisType _type;
        public RedisType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        private TimeSpan? _ttl;
        public TimeSpan? TTL
        {
            get => _ttl;
            set => SetProperty(ref _ttl, value);
        }

        private readonly IDatabase _database;

        public KeyInfo(IDatabase database, string name)
        {
            _database = database;
            _name = name;
        }

        public async Task<KeyInfo> LoadAsync()
        {
            var type = await _database.KeyTypeAsync(_name);
            var ttl = await _database.KeyTimeToLiveAsync(_name);

            Type = type;
            TTL = ttl;

            return this;
        }

        /// <summary>
        /// Delete key
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteAsync()
        {
            return await _database.KeyDeleteAsync(_name);
        }

        public async Task<string> GetValueByStringAsync()
        {
            return await _database.StringGetAsync(_name);
        }

        public async Task<bool> SetByStringAsync(string value)
        {
            return await _database.StringSetAsync(_name, value);
        }

        public async Task<bool> RenameAsync(string name)
        {
            return await _database.KeyRenameAsync(_name, name);
        }

        public async Task<bool> SetTTLAsync(DateTime? expiry)
        {
            return await _database.KeyExpireAsync(_name, expiry);
        }

        //public async Task<RedisValue[]> GetValueByListAsync(long start, long stop)
        //{
        //    return await _database.ListRangeAsync(_name, start, stop);
        //}

        public async Task<(int pageCount, int pageIndex, IEnumerable<KeyListValue> values)> GetValueByListAsync(int pageSize, int pageIndex)
        {
            if (pageIndex <= 0)
                pageIndex = 1;

            var pageCount = (int)(await _database.ListLengthAsync(_name)) / (pageSize + 1);
            var start = pageIndex == 1 ? 0 : ((pageIndex - 1) * (pageSize + 1));
            var stop = pageIndex == 1 ? pageSize : (start + pageSize);

            return (pageCount, pageIndex, (await _database.ListRangeAsync(_name, start, stop))
                .Select((value, index) => new KeyListValue { Index = start == 0 ? (index + 1) : (start + index), Value = value }));
        }

        public async Task<(int pageCount, int pageIndex, IEnumerable<KeyZsetValue> values)> GetValueByZsetAsync(int pageSize, int pageIndex)
        {
            if (pageIndex <= 0)
                pageIndex = 1;

            var pageCount = (int)(await _database.SortedSetLengthAsync(_name)) / (pageSize + 1);
            var start = pageIndex == 1 ? 0 : ((pageIndex - 1) * (pageSize + 1));
            var stop = pageIndex == 1 ? pageSize : (start + pageSize);

            var values = (await _database.SortedSetRangeByRankWithScoresAsync(_name, start, stop))
                .Select((value, index) => new KeyZsetValue { Index = start == 0 ? (index + 1) : (start + index), Value = value.Element, Score = value.Score });

            return (pageCount, pageIndex, values);
        }

        //public async Task<(long cursor, RedisValue[] values)> GetValueBySetAsync(long cursor, long count)
        //        {
        //            var results = (RedisResult[])await _database.ExecuteAsync("SSCAN", _name, cursor, "COUNT", count);
        //            var cCursor = int.Parse((string)results[0]);
        //            var values = ((RedisValue[])results[1]).ToArray();

        //            return (cCursor, values);
        //        }

        public async Task<StreamEntry[]> GetValueByStreamAsync(int count)
        {
            return await _database.StreamReadAsync(_name, "-", 1000);
        }
    }

    public class KeyListValue : BindableBase
    {
        private long _index;
        public long Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        private RedisValue _value;
        public RedisValue Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }

    public class KeyZsetValue : BindableBase
    {
        private long _index;
        public long Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        private RedisValue _value;
        public RedisValue Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        private double _score;
        public double Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }
    }

    public class KeyStreamValue : BindableBase
    {
        private long _index;
        public long Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        private double _id;
        public double Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private RedisValue _value;
        public RedisValue Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }

    public class KeyListValueCollection : ObservableCollection<KeyListValue>
    {
        public KeyListValueCollection(IEnumerable<KeyListValue> values) : base(values)
        {

        }
    }

    public class KeyZsetValueCollection : ObservableCollection<KeyZsetValue>
    {
        public KeyZsetValueCollection(IEnumerable<KeyZsetValue> values) : base(values)
        {

        }
    }

    public class KeyStreamValueCollection : ObservableCollection<KeyStreamValue>
    {
        public KeyStreamValueCollection(IEnumerable<KeyStreamValue> values) : base(values)
        {

        }
    }

    public class KeyCollection : ObservableCollection<KeyInfo>
    {
        private readonly IServer _server;
        private readonly IDatabase _database;

        public KeyCollection(IServer server, IDatabase database)
        {
            _server = server;
            _database = database;
        }

        public async Task LoadAsync()
        {
            var keys = new List<KeyInfo>();

            await foreach (var key in _server.KeysAsync(_database.Database))
                keys.Add(new KeyInfo(_database, key.ToString()));

            this.AddRange(keys.OrderBy(c => c.Name));
        }
    }
}