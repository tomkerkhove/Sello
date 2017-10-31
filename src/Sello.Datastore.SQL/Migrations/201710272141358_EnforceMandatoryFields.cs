using System.Data.Entity.Migrations;

namespace Sello.Datastore.SQL.Migrations
{
    public partial class EnforceMandatoryFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "EmailAddress", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "ConfirmationId", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Name", c => c.String());
            AlterColumn("dbo.Orders", "ConfirmationId", c => c.String());
            AlterColumn("dbo.Customers", "EmailAddress", c => c.Int(nullable: false));
            AlterColumn("dbo.Customers", "LastName", c => c.Int(nullable: false));
            AlterColumn("dbo.Customers", "FirstName", c => c.Int(nullable: false));
        }
    }
}
