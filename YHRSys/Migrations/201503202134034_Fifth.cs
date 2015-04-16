namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fifth : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Partners", "geoLongitude", c => c.String());
            AlterColumn("dbo.Partners", "geoLatitude", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Partners", "geoLatitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Partners", "geoLongitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
