using System.Threading.Tasks;

namespace RedisViewer.Core
{
    public interface IConnectionService
    {
        Task<bool> AddAsync(ConnectionInfo connection);
        Task<bool> RemoveAsync(ConnectionInfo connection);
        Task<ConnectionCollection> GetAllAsync();
    }
}