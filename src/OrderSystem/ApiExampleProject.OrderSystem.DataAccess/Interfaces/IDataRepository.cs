using System.Collections.Generic;
using System.Threading.Tasks;
using ApiExampleProject.Common.Pagination;

namespace ApiExampleProject.OrderSystem.DataAccess.Interfaces
{
    public interface IDataRepository<T>
        where T : class
    {
        Task<T> CreateItemAsync(T item);

        Task<IEnumerable<T>> ReadAllAsync();

        Task<IEnumerable<T>> ReadAllAsync(PaginationRequest paginationRequest);
    }
}
