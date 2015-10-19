namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fiftyone : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PartnerReportings",
                c => new
                    {
                        reportId = c.Int(nullable: false, identity: true),
                        activityId = c.Int(nullable: false),
                        reagentId = c.Int(nullable: false),
                        varietyId = c.Int(nullable: false),
                        reagentQty = c.Int(nullable: false),
                        varietyQty = c.Int(nullable: false),
                        comment = c.String(),
                        createdDate = c.DateTime(),
                        updatedDate = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        createdBy = c.String(),
                        updatedBy = c.String(),
                    })
                .PrimaryKey(t => t.reportId)
                .ForeignKey("dbo.PartnerActivities", t => t.activityId, cascadeDelete: true)
                .ForeignKey("dbo.Reagents", t => t.reagentId, cascadeDelete: true)
                .ForeignKey("dbo.Varieties", t => t.varietyId, cascadeDelete: true)
                .Index(t => t.activityId)
                .Index(t => t.reagentId)
                .Index(t => t.varietyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartnerReportings", "varietyId", "dbo.Varieties");
            DropForeignKey("dbo.PartnerReportings", "reagentId", "dbo.Reagents");
            DropForeignKey("dbo.PartnerReportings", "activityId", "dbo.PartnerActivities");
            DropIndex("dbo.PartnerReportings", new[] { "varietyId" });
            DropIndex("dbo.PartnerReportings", new[] { "reagentId" });
            DropIndex("dbo.PartnerReportings", new[] { "activityId" });
            DropTable("dbo.PartnerReportings");
        }
    }
}
