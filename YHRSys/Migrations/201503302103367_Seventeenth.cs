namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seventeenth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Partners", "contactCity", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Partners", "contactState", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Partners", "contactCountry", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Partners", "contactCountry");
            DropColumn("dbo.Partners", "contactState");
            DropColumn("dbo.Partners", "contactCity");
        }
    }
}
