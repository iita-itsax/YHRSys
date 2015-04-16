namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Twelvth : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.VarietyProcessFlows", "barcodeImageUrl", c => c.Binary());
            DropColumn("dbo.VarietyProcessFlows", "barcodeImageUrl"); 
            AddColumn("dbo.VarietyProcessFlows", "barcodeImageUrl", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VarietyProcessFlows", "barcodeImageUrl"); 
            //AlterColumn("dbo.VarietyProcessFlows", "barcodeImageUrl", c => c.String());
        }
    }
}
