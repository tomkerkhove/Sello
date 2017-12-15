using System.ComponentModel.DataAnnotations.Schema;

namespace Sello.Datastore.SQL.Model
{
    public class Order
    {
        public int Id { get; set; }
        public string ConfirmationId { get; set; }
        
        public virtual Customer Customer { get; set; }
        public int CustomerId { get; set; }
        
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
    }
}