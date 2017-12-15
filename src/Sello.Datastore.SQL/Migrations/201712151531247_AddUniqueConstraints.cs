using System.Data.Entity.Migrations;

namespace Sello.Datastore.SQL.Migrations
{
    public partial class AddUniqueConstraints : DbMigration
    {
        public override void Up()
        {
            AlterColumn("platform.Customers", "EmailAddress", c => c.String(nullable: false, maxLength: 600));
            AlterColumn("platform.Orders", "ConfirmationId", c => c.String(nullable: false, maxLength: 600));
            AlterColumn("platform.Products", "ExternalId", c => c.String(nullable: false, maxLength: 600));
            CreateIndex("platform.Customers", "EmailAddress", unique: true);
            CreateIndex("platform.Orders", "ConfirmationId", unique: true);
            CreateIndex("platform.Products", "ExternalId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("platform.Products", new[] { "ExternalId" });
            DropIndex("platform.Orders", new[] { "ConfirmationId" });
            DropIndex("platform.Customers", new[] { "EmailAddress" });
            AlterColumn("platform.Products", "ExternalId", c => c.String(nullable: false));
            AlterColumn("platform.Orders", "ConfirmationId", c => c.String(nullable: false));
            AlterColumn("platform.Customers", "EmailAddress", c => c.String(nullable: false));
        }
    }
}
