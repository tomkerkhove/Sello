using System.Data.Entity.Migrations;

namespace Sello.Datastore.SQL.Migrations
{
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Customers",
                    c => new
                    {
                        Id = c.Int(false, true),
                        FirstName = c.Int(false),
                        LastName = c.Int(false),
                        EmailAddress = c.Int(false)
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.Orders",
                    c => new
                    {
                        Id = c.Int(false, true),
                        ConfirmationId = c.String(),
                        CustomerId = c.Int(false)
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.Products",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(),
                        Description = c.String(),
                        Price = c.Double(false)
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.ProductOrders",
                    c => new
                    {
                        Product_Id = c.Int(false),
                        Order_Id = c.Int(false)
                    })
                .PrimaryKey(t => new {t.Product_Id, t.Order_Id})
                .ForeignKey("dbo.Products", t => t.Product_Id, true)
                .ForeignKey("dbo.Orders", t => t.Order_Id, true)
                .Index(t => t.Product_Id)
                .Index(t => t.Order_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.ProductOrders", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.ProductOrders", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductOrders", new[] {"Order_Id"});
            DropIndex("dbo.ProductOrders", new[] {"Product_Id"});
            DropTable("dbo.ProductOrders");
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
            DropTable("dbo.Customers");
        }
    }
}