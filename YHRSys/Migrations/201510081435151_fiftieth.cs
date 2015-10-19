namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fiftieth : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PartnerActivities", "varietyQty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PartnerActivities", "varietyQty", c => c.Int());
        }
    }
}
