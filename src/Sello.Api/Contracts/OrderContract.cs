using System.Collections.Generic;

namespace Sello.Api.Contracts
{
    public class OrderContract
    {
        public CustomerContract Customer { get; set; }
        public ProductInformationContract Product { get; set; }
    }
}