using System.Threading.Tasks;

namespace RedisViewer.Core
{
    public interface IConnectionService
    {
        Task<bool> AddAsync(ConnectionInfo connection);
        Task<ConnectionCollection> GetAllAsync();
    }
}