namespace Sello.Datastore.SQL.Model
{
    public class Order
    {
        public string ConfirmationId { get; set; }
        public virtual Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public int Id { get; set; }
        public virtual Product Product { get; set; }
        public string ProductId { get; set; }
    }
}