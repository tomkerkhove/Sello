using System.Collections.Generic;

namespace Sello.Api.Contracts
{
    public class OrderConfirmationContract
    {
        public string ConfirmationId { get; set; }
        public virtual OrderItemContract Item { get; set; }
        public CustomerContract Customer { get; set; }
        public double TotalAmount { get; set; }
    }
}