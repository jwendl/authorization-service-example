using System.Threading;
using System.Threading.Tasks;
using ApiExampleProject.CustomerData.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ApiExampleProject.CustomerData.DataAccess.Interfaces
{
    public interface IDataContext
    {
        DbSet<Customer> Customers { get; set; }

        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class;

        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
