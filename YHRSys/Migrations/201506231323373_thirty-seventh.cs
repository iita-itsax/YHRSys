namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirtyseventh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityAchievements",
                c => new
                    {
                        achievementId = c.Int(nullable: false, identity: true),
                        activityLogId = c.Int(nullable: false),
                        achievementDate = c.DateTime(),
                        description = c.String(),
                        createdDate = c.DateTime(),
                        updatedDate = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        createdBy = c.String(),
                        updatedBy = c.String(),
                    })
                .PrimaryKey(t => t.achievementId)
                .ForeignKey("dbo.WeeklyActivityLogs", t => t.activityLogId, cascadeDelete: true)
                .Index(t => t.activityLogId);
            
            CreateTable(
                "dbo.ActivityWorkplans",
                c => new
                    {
                        workplanId = c.Int(nullable: false, identity: true),
                        StartPeriod = c.DateTime(),
                        EndPeriod = c.DateTime(),
                        Objective = c.String(nullable: false, maxLength: 255),
                        PerformanceIndicator = c.String(nullable: false, maxLength: 255),
                        createdDate = c.DateTime(),
                        updatedDate = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        createdBy = c.String(),
                        updatedBy = c.String(),
                    })
                .PrimaryKey(t => t.workplanId);
            
            AddColumn("dbo.WeeklyActivityLogs", "workplanId", c => c.Int(nullable: false));
            AddColumn("dbo.WeeklyActivityLogs", "status", c => c.String());
            CreateIndex("dbo.WeeklyActivityLogs", "workplanId");
            AddForeignKey("dbo.WeeklyActivityLogs", "workplanId", "dbo.ActivityWorkplans", "workplanId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WeeklyActivityLogs", "workplanId", "dbo.ActivityWorkplans");
            DropForeignKey("dbo.ActivityAchievements", "activityLogId", "dbo.WeeklyActivityLogs");
            DropIndex("dbo.WeeklyActivityLogs", new[] { "workplanId" });
            DropIndex("dbo.ActivityAchievements", new[] { "activityLogId" });
            DropColumn("dbo.WeeklyActivityLogs", "status");
            DropColumn("dbo.WeeklyActivityLogs", "workplanId");
            DropTable("dbo.ActivityWorkplans");
            DropTable("dbo.ActivityAchievements");
        }
    }
}
