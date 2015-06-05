namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twenthysixth : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocationSubordinates",
                c => new
                    {
                        subordinateId = c.Int(nullable: false, identity: true),
                        locationUserId = c.Int(nullable: false),
                        userId = c.String(maxLength: 128),
                        status = c.Int(nullable: false),
                        workBrief = c.String(),
                    })
                .PrimaryKey(t => t.subordinateId)
                .ForeignKey("dbo.LocationUsers", t => t.locationUserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.userId)
                .Index(t => t.locationUserId)
                .Index(t => t.userId);
            
            AddColumn("dbo.Activities", "status", c => c.Int(nullable: false));
            AddColumn("dbo.InternalReagentUsages", "receivedBy", c => c.String());
            AddColumn("dbo.PartnerActivities", "giverId", c => c.Int(nullable: true)); //Changed from false to true to be able to accommodate two properties in the same model referencing the same model class (Partner) without throwing this EXCEPTION - The ALTER TABLE statement conflicted with the FOREIGN KEY constraint "FK_dbo.PartnerActivities_dbo.Partners_giverId". The conflict occurred in database "ITSAGlobalSysDB", table "dbo.Partners", column 'partnerId'.
            AddColumn("dbo.PartnerActivities", "seedsAvailable", c => c.Int());
            AddColumn("dbo.PartnerActivities", "seedsGiven", c => c.Int());
            CreateIndex("dbo.PartnerActivities", "giverId");
            AddForeignKey("dbo.PartnerActivities", "giverId", "dbo.Partners", "partnerId", cascadeDelete: false); //Changed from true to false to accommodate two properties in the same model  referencing the same model class (Partner) without throwing this EXCEPTION - Introducing FOREIGN KEY constraint 'FK_dbo.PartnerActivities_dbo.Partners_giverId' on table 'PartnerActivities' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints. Could not create constraint. See previous errors.
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartnerActivities", "giverId", "dbo.Partners");
            DropForeignKey("dbo.LocationSubordinates", "userId", "dbo.AspNetUsers");
            DropForeignKey("dbo.LocationSubordinates", "locationUserId", "dbo.LocationUsers");
            DropIndex("dbo.PartnerActivities", new[] { "giverId" });
            DropIndex("dbo.LocationSubordinates", new[] { "userId" });
            DropIndex("dbo.LocationSubordinates", new[] { "locationUserId" });
            DropColumn("dbo.PartnerActivities", "seedsGiven");
            DropColumn("dbo.PartnerActivities", "seedsAvailable");
            DropColumn("dbo.PartnerActivities", "giverId");
            DropColumn("dbo.InternalReagentUsages", "receivedBy");
            DropColumn("dbo.Activities", "status");
            DropTable("dbo.LocationSubordinates");
        }
    }
}
