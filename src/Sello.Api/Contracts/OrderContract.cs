using System.Collections.Generic;

namespace Sello.Api.Contracts
{
    public class OrderContract
    {
        public virtual List<ProductContract> Products { get; set; }
        public CustomerContract Customer { get; set; }
    }
}