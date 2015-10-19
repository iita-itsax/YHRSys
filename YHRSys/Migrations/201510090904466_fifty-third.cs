namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fiftythird : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PartnerReportings", "reagentId", "dbo.Reagents");
            DropForeignKey("dbo.PartnerReportings", "varietyId", "dbo.Varieties");
            DropIndex("dbo.PartnerReportings", new[] { "reagentId" });
            DropIndex("dbo.PartnerReportings", new[] { "varietyId" });
            AddColumn("dbo.PartnerReportings", "reportDate", c => c.DateTime());
            DropColumn("dbo.PartnerReportings", "reagentId");
            DropColumn("dbo.PartnerReportings", "varietyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PartnerReportings", "varietyId", c => c.Int(nullable: false));
            AddColumn("dbo.PartnerReportings", "reagentId", c => c.Int(nullable: false));
            DropColumn("dbo.PartnerReportings", "reportDate");
            CreateIndex("dbo.PartnerReportings", "varietyId");
            CreateIndex("dbo.PartnerReportings", "reagentId");
            AddForeignKey("dbo.PartnerReportings", "varietyId", "dbo.Varieties", "varietyId", cascadeDelete: true);
            AddForeignKey("dbo.PartnerReportings", "reagentId", "dbo.Reagents", "reagentId", cascadeDelete: true);
        }
    }
}
