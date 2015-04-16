namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sixth : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PartnerActivities", "officerInCharge_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PartnerActivities", new[] { "officerInCharge_Id" });
            AddColumn("dbo.PartnerActivities", "userId", c => c.String(maxLength: 128));
            CreateIndex("dbo.PartnerActivities", "userId");
            AddForeignKey("dbo.PartnerActivities", "userId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.PartnerActivities", "officerInCharge_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PartnerActivities", "officerInCharge_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.PartnerActivities", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.PartnerActivities", new[] { "userId" });
            DropColumn("dbo.PartnerActivities", "userId");
            CreateIndex("dbo.PartnerActivities", "officerInCharge_Id");
            AddForeignKey("dbo.PartnerActivities", "officerInCharge_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
