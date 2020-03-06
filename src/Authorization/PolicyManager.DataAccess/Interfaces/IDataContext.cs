using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.DataAccess.Interfaces
{
    public interface IDataContext
    {
        DbSet<Thing> Things { get; set; }

        DbSet<ThingAttribute> ThingAttributes { get; set; }

        DbSet<ThingPolicy> ThingPolicies { get; set; }

        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class;


        [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "<Pending>")]
        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
