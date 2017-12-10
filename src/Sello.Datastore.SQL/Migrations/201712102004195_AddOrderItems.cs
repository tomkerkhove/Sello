using System.Data.Entity.Migrations;

namespace Sello.Datastore.SQL.Migrations
{
    public partial class AddOrderItems : DbMigration
    {
        public override void Down()
        {
            CreateTable(
                    "dbo.ProductOrders",
                    c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        Order_Id = c.Int(nullable: false)
                    })
                .PrimaryKey(t => new {t.Product_Id, t.Order_Id});

            DropForeignKey("dbo.OrderItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Orders", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropIndex("dbo.Orders", new[] {"Product_Id"});
            DropIndex("dbo.OrderItems", new[] {"ProductId"});
            DropIndex("dbo.OrderItems", new[] {"OrderId"});
            DropColumn("dbo.Orders", "Product_Id");
            DropTable("dbo.OrderItems");
            CreateIndex("dbo.ProductOrders", "Order_Id");
            CreateIndex("dbo.ProductOrders", "Product_Id");
            AddForeignKey("dbo.ProductOrders", "Order_Id", "dbo.Orders", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProductOrders", "Product_Id", "dbo.Products", "Id", cascadeDelete: true);
        }

        public override void Up()
        {
            DropForeignKey("dbo.ProductOrders", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductOrders", "Order_Id", "dbo.Orders");
            DropIndex("dbo.ProductOrders", new[] {"Product_Id"});
            DropIndex("dbo.ProductOrders", new[] {"Order_Id"});
            CreateTable(
                    "dbo.OrderItems",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);

            AddColumn("dbo.Orders", "Product_Id", c => c.Int());
            CreateIndex("dbo.Orders", "Product_Id");
            AddForeignKey("dbo.Orders", "Product_Id", "dbo.Products", "Id");
            DropTable("dbo.ProductOrders");
        }
    }
}