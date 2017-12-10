namespace Sello.Api.Contracts
{
    public class OrderItemContract
    {
        public int Amount { get; set; }
        public double Price { get; set; }
        public string ProductId { get; set; }
    }
}