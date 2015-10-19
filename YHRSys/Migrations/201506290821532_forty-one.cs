namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fortyone : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activities", "activityDefinitionId", c => c.Int(nullable: false));
            AlterColumn("dbo.Activities", "locationId", c => c.Int(nullable: false));
            AlterColumn("dbo.Activities", "typeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Activities", "varietyId", c => c.Int());
            AlterColumn("dbo.Activities", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.ActivityAssignments", "activityId", c => c.Int(nullable: false));
            AlterColumn("dbo.ActivityAssignments", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String());
            AlterColumn("dbo.AspNetUserClaims", "User_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserLogins", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserRoles", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserRoles", "RoleId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ApplicationUserGroups", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ApplicationUserGroups", "GroupId", c => c.Int(nullable: false));
            AlterColumn("dbo.ApplicationRoleGroups", "RoleId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ApplicationRoleGroups", "GroupId", c => c.Int(nullable: false));
            AlterColumn("dbo.LocationUsers", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.LocationUsers", "locationId", c => c.Int(nullable: false));
            AlterColumn("dbo.LocationSubordinates", "locationUserId", c => c.Int(nullable: false));
            AlterColumn("dbo.LocationSubordinates", "userSubordinateId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Varieties", "varietyDefinitionId", c => c.Int(nullable: false));
            AlterColumn("dbo.Varieties", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Varieties", "locationId", c => c.Int());
            AlterColumn("dbo.Varieties", "uomId", c => c.Int());
            AlterColumn("dbo.Varieties", "specieId", c => c.Int());
            AlterColumn("dbo.VarietyProcessFlows", "varietyId", c => c.Int(nullable: false));
            AlterColumn("dbo.VarietyProcessFlows", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.ActivityAchievements", "activityLogId", c => c.Int(nullable: false));
            AlterColumn("dbo.WeeklyActivityLogs", "workplanId", c => c.Int(nullable: false));
            AlterColumn("dbo.WeeklyActivityLogs", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.InternalReagentUsages", "reagentId", c => c.Int(nullable: false));
            AlterColumn("dbo.InternalReagentUsages", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Reagents", "measurementId", c => c.Int(nullable: false));
            AlterColumn("dbo.Inventories", "reagentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Inventories", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.PartnerActivities", "partnerId", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnerActivities", "giverId", c => c.Int(nullable: true));
            AlterColumn("dbo.PartnerActivities", "reagentId", c => c.Int());
            AlterColumn("dbo.PartnerActivities", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.PartnerContacts", "partnerId", c => c.Int(nullable: false));
            AlterColumn("dbo.SiteContents", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Stocks", "reagentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stocks", "reagentId", c => c.Int(nullable: false));
            AlterColumn("dbo.SiteContents", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.PartnerContacts", "partnerId", c => c.Int(nullable: false));
            AlterColumn("dbo.PartnerActivities", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.PartnerActivities", "reagentId", c => c.Int());
            AlterColumn("dbo.PartnerActivities", "giverId", c => c.Int(nullable: true));
            AlterColumn("dbo.PartnerActivities", "partnerId", c => c.Int(nullable: false));
            AlterColumn("dbo.Inventories", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Inventories", "reagentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Reagents", "measurementId", c => c.Int(nullable: false));
            AlterColumn("dbo.InternalReagentUsages", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.InternalReagentUsages", "reagentId", c => c.Int(nullable: false));
            AlterColumn("dbo.WeeklyActivityLogs", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.WeeklyActivityLogs", "workplanId", c => c.Int(nullable: false));
            AlterColumn("dbo.ActivityAchievements", "activityLogId", c => c.Int(nullable: false));
            AlterColumn("dbo.VarietyProcessFlows", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.VarietyProcessFlows", "varietyId", c => c.Int(nullable: false));
            AlterColumn("dbo.Varieties", "specieId", c => c.Int());
            AlterColumn("dbo.Varieties", "uomId", c => c.Int());
            AlterColumn("dbo.Varieties", "locationId", c => c.Int());
            AlterColumn("dbo.Varieties", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Varieties", "varietyDefinitionId", c => c.Int(nullable: false));
            AlterColumn("dbo.LocationSubordinates", "userSubordinateId", c => c.String(maxLength: 128));
            AlterColumn("dbo.LocationSubordinates", "locationUserId", c => c.Int(nullable: false));
            AlterColumn("dbo.LocationUsers", "locationId", c => c.Int(nullable: false));
            AlterColumn("dbo.LocationUsers", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.ApplicationRoleGroups", "GroupId", c => c.Int(nullable: false));
            AlterColumn("dbo.ApplicationRoleGroups", "RoleId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ApplicationUserGroups", "GroupId", c => c.Int(nullable: false));
            AlterColumn("dbo.ApplicationUserGroups", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserRoles", "RoleId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserRoles", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserLogins", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserClaims", "User_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false));
            AlterColumn("dbo.ActivityAssignments", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.ActivityAssignments", "activityId", c => c.Int(nullable: false));
            AlterColumn("dbo.Activities", "userId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Activities", "varietyId", c => c.Int());
            AlterColumn("dbo.Activities", "typeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Activities", "locationId", c => c.Int(nullable: false));
            AlterColumn("dbo.Activities", "activityDefinitionId", c => c.Int(nullable: false));
        }
    }
}
