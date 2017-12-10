using System.Collections.Generic;

namespace Sello.Datastore.SQL.Model
{
    public class Order
    {
        public int Id { get; set; }
        public string ConfirmationId { get; set; }
        public virtual List<OrderItem> Items { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}