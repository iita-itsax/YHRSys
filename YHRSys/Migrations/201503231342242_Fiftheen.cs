namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fiftheen : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PartnerActivities", "reagentId", "dbo.Reagents");
            DropIndex("dbo.PartnerActivities", new[] { "reagentId" });
            AlterColumn("dbo.PartnerActivities", "reagentId", c => c.Int());
            AlterColumn("dbo.PartnerActivities", "reagentQty", c => c.Int());
            AlterColumn("dbo.PartnerActivities", "tcPlantletsGiven", c => c.Int());
            AlterColumn("dbo.PartnerActivities", "bioreactorplantsGiven", c => c.Int());
            AlterColumn("dbo.PartnerActivities", "tubersGiven", c => c.Int());
            AlterColumn("dbo.PartnerActivities", "tcPlantletsAvailable", c => c.Int());
            AlterColumn("dbo.PartnerActivities", "tibPlantletsAvailable", c => c.Int());
            AlterColumn("dbo.PartnerActivities", "tubersAvailable", c => c.Int());
            CreateIndex("dbo.PartnerActivities", "reagentId");
            AddForeignKey("dbo.PartnerActivities", "reagentId", "dbo.Reagents", "reagentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartnerActivities", "reagentId", "dbo.Reagents");
            DropIndex("dbo.PartnerActivities", new[] { "reagentId" });
            AlterColumn("dbo.PartnerActivities", "tubersAvailable", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnerActivities", "tibPlantletsAvailable", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnerActivities", "tcPlantletsAvailable", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnerActivities", "tubersGiven", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnerActivities", "bioreactorplantsGiven", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnerActivities", "tcPlantletsGiven", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnerActivities", "reagentQty", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnerActivities", "reagentId", c => c.Int(nullable: false));
            CreateIndex("dbo.PartnerActivities", "reagentId");
            AddForeignKey("dbo.PartnerActivities", "reagentId", "dbo.Reagents", "reagentId", cascadeDelete: true);
        }
    }
}
