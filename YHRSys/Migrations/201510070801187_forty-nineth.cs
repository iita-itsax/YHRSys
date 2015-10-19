namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fortynineth : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Seedlings",
                c => new
                    {
                        seedlingId = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        createdDate = c.DateTime(),
                        updatedDate = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        createdBy = c.String(),
                        updatedBy = c.String(),
                    })
                .PrimaryKey(t => t.seedlingId);
            
            AddColumn("dbo.PartnerActivities", "varietyId", c => c.Int());
            AddColumn("dbo.PartnerActivities", "varietyQty", c => c.Int());
            CreateIndex("dbo.PartnerActivities", "varietyId");
            AddForeignKey("dbo.PartnerActivities", "varietyId", "dbo.Varieties", "varietyId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartnerActivities", "varietyId", "dbo.Varieties");
            DropIndex("dbo.PartnerActivities", new[] { "varietyId" });
            DropColumn("dbo.PartnerActivities", "varietyQty");
            DropColumn("dbo.PartnerActivities", "varietyId");
            DropTable("dbo.Seedlings");
        }
    }
}
