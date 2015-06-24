namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirtieth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Varieties", "availableQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.Varieties", "totalWeight", c => c.Int(nullable: false));
            AddColumn("dbo.Varieties", "uomId", c => c.Int());
            AddColumn("dbo.VarietyProcessFlows", "quality", c => c.Int(nullable: false));
            AlterColumn("dbo.Activities", "status", c => c.String());
            CreateIndex("dbo.Varieties", "uomId");
            AddForeignKey("dbo.Varieties", "uomId", "dbo.Measurements", "measurementId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Varieties", "uomId", "dbo.Measurements");
            DropIndex("dbo.Varieties", new[] { "uomId" });
            AlterColumn("dbo.Activities", "status", c => c.Int(nullable: false));
            DropColumn("dbo.VarietyProcessFlows", "quality");
            DropColumn("dbo.Varieties", "uomId");
            DropColumn("dbo.Varieties", "totalWeight");
            DropColumn("dbo.Varieties", "availableQuantity");
        }
    }
}
