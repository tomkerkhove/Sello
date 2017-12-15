namespace Sello.Api.Contracts
{
    public class OrderConfirmationContract
    {
        public string ConfirmationId { get; set; }
        public OrderContract Order { get; set; }
    }
}