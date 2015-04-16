namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Eleventh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VarietyProcessFlows", "barcodeImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VarietyProcessFlows", "barcodeImageUrl");
        }
    }
}
