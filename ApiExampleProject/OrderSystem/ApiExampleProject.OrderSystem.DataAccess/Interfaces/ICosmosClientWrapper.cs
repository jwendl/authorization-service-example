using System.Threading.Tasks;
using ApiExampleProject.OrderSystem.DataAccess.Models;

namespace ApiExampleProject.OrderSystem.DataAccess.Interfaces
{
    public interface ICosmosClientWrapper<T>
        where T : BaseCosmosDocument
    {
        Task CreateDatabaseAsync();

        Task CreateContainerAsync();

        Task<T> CreateItemAsync(T item);
    }
}
