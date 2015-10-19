namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fiftyfive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PartnerReportings", "spQty", c => c.Int(nullable: false));
            AddColumn("dbo.PartnerReportings", "tcpQty", c => c.Int(nullable: false));
            AddColumn("dbo.PartnerReportings", "tpQty", c => c.Int(nullable: false));
            AddColumn("dbo.PartnerReportings", "bioRPQty", c => c.Int(nullable: false));
            DropColumn("dbo.PartnerReportings", "varietyQty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PartnerReportings", "varietyQty", c => c.Int(nullable: false));
            DropColumn("dbo.PartnerReportings", "bioRPQty");
            DropColumn("dbo.PartnerReportings", "tpQty");
            DropColumn("dbo.PartnerReportings", "tcpQty");
            DropColumn("dbo.PartnerReportings", "spQty");
        }
    }
}
