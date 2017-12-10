﻿namespace Sello.Datastore.SQL.Model
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public virtual Order Order { get; set; }
        public int OrderId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
    }
}