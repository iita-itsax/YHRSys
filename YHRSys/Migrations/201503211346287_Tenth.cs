namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tenth : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PartnerContacts", "geoLongitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.PartnerContacts", "geoLatitude", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PartnerContacts", "geoLatitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.PartnerContacts", "geoLongitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
