using System;
using System.Threading.Tasks;
using ApiExampleProject.Common.Configuration;
using ApiExampleProject.OrderSystem.DataAccess.Interfaces;
using ApiExampleProject.OrderSystem.DataAccess.Models;
using ApiExampleProject.OrderSystem.DataAccess.Resources;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApiExampleProject.OrderSystem.DataAccess.Repositories
{
    public class CosmosClientWrapper<T>
        : ICosmosClientWrapper<T>, IDisposable
        where T : BaseCosmosDocument
    {
        private Container container;
        private Database database;

        private readonly ILogger<CosmosClientWrapper<T>> logger;
        private readonly CosmosClient cosmosClient;
        private readonly string databaseId;
        private readonly string containerId;

        // Track whether Dispose has been called.
        private bool disposed = false;

        public CosmosClientWrapper(ILogger<CosmosClientWrapper<T>> logger, IOptions<CosmosConfiguration> options, string databaseId, string containerId)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options));

            var cosmosConfiguration = options.Value;
            cosmosClient = new CosmosClient(cosmosConfiguration.EndpointLocation, cosmosConfiguration.PrimaryKey);

            this.logger = logger;
            this.databaseId = databaseId;
            this.containerId = containerId;
        }

        public async Task CreateDatabaseAsync()
        {
            // Create a new database
            var databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            database = databaseResponse.Database;

            logger.LogInformation(OrderResources.CreateDatabaseLogMessage, databaseId);
        }

        public async Task CreateContainerAsync()
        {
            var containerResponse = await database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");
            container = containerResponse.Container;

            logger.LogInformation(OrderResources.CreateContainerLogMessage, containerId);
        }

        public async Task<T> CreateItemAsync(T item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));

            var itemResponse = await container.CreateItemAsync(item, new PartitionKey(item.PartitionKey));
            return itemResponse.Resource;
        }

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    cosmosClient.Dispose();
                }

                // Note disposing has been done.
                disposed = true;
            }
        }
    }
}
