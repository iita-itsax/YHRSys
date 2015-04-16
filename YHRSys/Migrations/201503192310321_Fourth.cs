namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fourth : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VarietyProcessFlows", "userId_Id", "dbo.AspNetUsers");
            DropIndex("dbo.VarietyProcessFlows", new[] { "userId_Id" });
            AddColumn("dbo.VarietyProcessFlows", "userId", c => c.String(maxLength: 128));
            CreateIndex("dbo.VarietyProcessFlows", "userId");
            AddForeignKey("dbo.VarietyProcessFlows", "userId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.VarietyProcessFlows", "userId_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VarietyProcessFlows", "userId_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.VarietyProcessFlows", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.VarietyProcessFlows", new[] { "userId" });
            DropColumn("dbo.VarietyProcessFlows", "userId");
            CreateIndex("dbo.VarietyProcessFlows", "userId_Id");
            AddForeignKey("dbo.VarietyProcessFlows", "userId_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
