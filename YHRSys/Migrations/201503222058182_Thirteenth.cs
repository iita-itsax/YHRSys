namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Thirteenth : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VarietyProcessFlows", "form", c => c.String(nullable: false));
            AlterColumn("dbo.VarietyProcessFlows", "rank", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VarietyProcessFlows", "rank", c => c.String());
            AlterColumn("dbo.VarietyProcessFlows", "form", c => c.String());
        }
    }
}
