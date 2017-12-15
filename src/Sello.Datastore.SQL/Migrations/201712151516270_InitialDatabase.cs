using System.Data.Entity.Migrations;

namespace Sello.Datastore.SQL.Migrations
{
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "platform.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "platform.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConfirmationId = c.String(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("platform.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("platform.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "platform.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternalId = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("platform.Orders", "ProductId", "platform.Products");
            DropForeignKey("platform.Orders", "CustomerId", "platform.Customers");
            DropIndex("platform.Orders", new[] { "ProductId" });
            DropIndex("platform.Orders", new[] { "CustomerId" });
            DropTable("platform.Products");
            DropTable("platform.Orders");
            DropTable("platform.Customers");
        }
    }
}
