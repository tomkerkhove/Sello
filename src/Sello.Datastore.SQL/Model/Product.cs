using System.Collections.Generic;

namespace Sello.Datastore.SQL.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}