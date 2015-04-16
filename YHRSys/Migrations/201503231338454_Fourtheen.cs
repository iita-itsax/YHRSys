namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fourtheen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PartnerActivities", "reagentQty", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PartnerActivities", "reagentQty");
        }
    }
}
