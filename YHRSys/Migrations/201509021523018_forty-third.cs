namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fortythird : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Forms",
                c => new
                    {
                        formId = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        createdDate = c.DateTime(),
                        updatedDate = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        createdBy = c.String(),
                        updatedBy = c.String(),
                    })
                .PrimaryKey(t => t.formId);
            
            CreateTable(
                "dbo.Ranks",
                c => new
                    {
                        rankId = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        createdDate = c.DateTime(),
                        updatedDate = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        createdBy = c.String(),
                        updatedBy = c.String(),
                    })
                .PrimaryKey(t => t.rankId);
            
            AddColumn("dbo.VarietyProcessFlows", "formId", c => c.Int(nullable: true));
            AddColumn("dbo.VarietyProcessFlows", "rankId", c => c.Int(nullable: true));
            CreateIndex("dbo.VarietyProcessFlows", "formId");
            CreateIndex("dbo.VarietyProcessFlows", "rankId");
            AddForeignKey("dbo.VarietyProcessFlows", "formId", "dbo.Forms", "formId", cascadeDelete: true);
            AddForeignKey("dbo.VarietyProcessFlows", "rankId", "dbo.Ranks", "rankId", cascadeDelete: true);
            DropColumn("dbo.VarietyProcessFlows", "form");
            DropColumn("dbo.VarietyProcessFlows", "rank");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VarietyProcessFlows", "rank", c => c.String(nullable: false));
            AddColumn("dbo.VarietyProcessFlows", "form", c => c.String(nullable: false));
            DropForeignKey("dbo.VarietyProcessFlows", "rankId", "dbo.Ranks");
            DropForeignKey("dbo.VarietyProcessFlows", "formId", "dbo.Forms");
            DropIndex("dbo.VarietyProcessFlows", new[] { "rankId" });
            DropIndex("dbo.VarietyProcessFlows", new[] { "formId" });
            DropColumn("dbo.VarietyProcessFlows", "rankId");
            DropColumn("dbo.VarietyProcessFlows", "formId");
            DropTable("dbo.Ranks");
            DropTable("dbo.Forms");
        }
    }
}
