using System.Collections.Generic;

namespace Sello.Api.Contracts
{
    public class OrderConfirmationContract
    {
        public string ConfirmationId { get; set; }
        public virtual List<OrderItemContract> Items { get; set; }
        public CustomerContract Customer { get; set; }
        public double TotalAmount { get; set; }
    }
}