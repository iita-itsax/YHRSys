namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twenthyseventh : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LocationSubordinates", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.LocationSubordinates", new[] { "userId" });
            DropTable("dbo.LocationSubordinates");
            CreateTable(
                "dbo.LocationSubordinates",
                c => new
                    {
                        subordinateId = c.Int(nullable: false, identity: true),
                        locationUserId = c.Int(nullable: false),
                        userSubordinateId = c.String(maxLength: 128),
                        status = c.Int(nullable: false),
                        workBrief = c.String(),
                        createdDate = c.DateTime(),
                        updatedDate = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        createdBy = c.String(),
                        updatedBy = c.String(),
                    })
                .PrimaryKey(t => t.subordinateId)
                .ForeignKey("dbo.AspNetUsers", t => t.userSubordinateId)
                .Index(t => t.userSubordinateId);
            
            CreateTable(
                "dbo.ActivityAssignments",
                c => new
                    {
                        assignmentId = c.Int(nullable: false, identity: true),
                        activityId = c.Int(nullable: false),
                        userId = c.String(maxLength: 128),
                        todo = c.String(),
                        createdDate = c.DateTime(),
                        updatedDate = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        createdBy = c.String(),
                        updatedBy = c.String(),
                    })
                .PrimaryKey(t => t.assignmentId)
                .ForeignKey("dbo.Activities", t => t.activityId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.userId)
                .Index(t => t.activityId)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.WeeklyActivityLogs",
                c => new
                    {
                        activityLogId = c.Int(nullable: false, identity: true),
                        userId = c.String(maxLength: 128),
                        startDate = c.DateTime(),
                        endDate = c.DateTime(),
                        description = c.String(),
                        createdDate = c.DateTime(),
                        updatedDate = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        createdBy = c.String(),
                        updatedBy = c.String(),
                    })
                .PrimaryKey(t => t.activityLogId)
                .ForeignKey("dbo.AspNetUsers", t => t.userId)
                .Index(t => t.userId);
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.subordinateId);
            
            DropForeignKey("dbo.WeeklyActivityLogs", "userId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ActivityAssignments", "userId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ActivityAssignments", "activityId", "dbo.Activities");
            DropForeignKey("dbo.LocationSubordinates", "userSubordinateId", "dbo.AspNetUsers");
            DropIndex("dbo.WeeklyActivityLogs", new[] { "userId" });
            DropIndex("dbo.ActivityAssignments", new[] { "userId" });
            DropIndex("dbo.ActivityAssignments", new[] { "activityId" });
            DropIndex("dbo.LocationSubordinates", new[] { "userSubordinateId" });
            DropTable("dbo.WeeklyActivityLogs");
            DropTable("dbo.ActivityAssignments");
            DropTable("dbo.LocationSubordinates");
            CreateIndex("dbo.LocationSubordinates", "userId");
            AddForeignKey("dbo.LocationSubordinates", "userId", "dbo.AspNetUsers", "Id");
        }
    }
}
