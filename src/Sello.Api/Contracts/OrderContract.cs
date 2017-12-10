using System.Collections.Generic;

namespace Sello.Api.Contracts
{
    public class OrderContract
    {
        public CustomerContract Customer { get; set; }
        public virtual List<OrderItemContract> Items { get; set; }
        public double TotalAmount { get; set; }
    }
}