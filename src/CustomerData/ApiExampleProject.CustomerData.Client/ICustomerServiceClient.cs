using System.Threading.Tasks;
using ApiExampleProject.CustomerData.DataAccess.Models;
using Refit;

namespace ApiExampleProject.CustomerData.Client
{
    public interface ICustomerServiceClient
    {
        [Headers("Authorization: Bearer")]
        [Post("/api/customers")]
        Task<Customer> CreateCustomerAsync([Body] Customer customer);
    }
}
