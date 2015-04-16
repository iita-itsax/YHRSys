namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Eight : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PartnerContacts", "title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PartnerContacts", "title");
        }
    }
}
