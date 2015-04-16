namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nineth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PartnerContacts", "personTitle", c => c.String());
            DropColumn("dbo.PartnerContacts", "title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PartnerContacts", "title", c => c.String());
            DropColumn("dbo.PartnerContacts", "personTitle");
        }
    }
}
