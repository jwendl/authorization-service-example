using System;
using System.Collections.Generic;

namespace ApiExampleProject.OrderSystem.DataAccess.Models
{
    public class Order
        : BaseCosmosDocument
    {
        public DateTime OrderDate { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
