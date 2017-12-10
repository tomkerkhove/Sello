using System.Data.Entity.Migrations;

namespace Sello.Datastore.SQL.Migrations
{
    public partial class IntroduceExternalIdForProduct : DbMigration
    {
        public override void Down()
        {
            DropColumn("dbo.Products", "ExternalId");
        }

        public override void Up()
        {
            AddColumn("dbo.Products", "ExternalId", c => c.String());
        }
    }
}