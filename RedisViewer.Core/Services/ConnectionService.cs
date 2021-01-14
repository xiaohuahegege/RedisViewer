using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RedisViewer.Core
{
    internal class ConnectionService : IConnectionService
    {
        private string _path;
        private Encoding _encoding;

        public ConnectionService()
        {
            _encoding = Encoding.UTF8;
            _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Constants.Application_Id, Constants.Connections_File_Name);
        }

        public async Task<bool> AddAsync(ConnectionInfo connection)
        {
            var connections = (await GetAllAsync()) ?? new ConnectionCollection();
            connections.Add(connection);

            return await _path.EnsureCreateDirectory()
                .WriteJsonToFileAsync(connections.ToJsonString(Formatting.Indented), _encoding);
        }

        public async Task<bool> RemoveAsync(ConnectionInfo connection)
        {
            return false;
        }

        public void UpdateAsync(ConnectionInfo connection)
        {

        }

        public async Task<ConnectionCollection> GetAllAsync()
        {
            return (await _path.ReadJsonFromFileAsync(_encoding))?.ToJsonObject<ConnectionCollection>();
        }
    }
}