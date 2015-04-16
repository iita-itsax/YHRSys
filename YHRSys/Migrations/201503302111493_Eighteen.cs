namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Eighteen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PartnerContacts", "contactCity", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.PartnerContacts", "contactState", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.PartnerContacts", "contactCountry", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PartnerContacts", "contactCountry");
            DropColumn("dbo.PartnerContacts", "contactState");
            DropColumn("dbo.PartnerContacts", "contactCity");
        }
    }
}
