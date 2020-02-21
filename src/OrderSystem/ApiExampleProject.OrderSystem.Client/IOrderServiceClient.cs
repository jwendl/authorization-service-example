using System.Threading.Tasks;
using ApiExampleProject.OrderSystem.DataAccess.Models;
using Refit;

namespace ApiExampleProject.OrderSystem.Client
{
    public interface IOrderServiceClient
    {
        [Headers("Authorization: Bearer")]
        [Post("/api/customers")]
        Task<Order> CreateOrderAsync(Order order);
    }
}
