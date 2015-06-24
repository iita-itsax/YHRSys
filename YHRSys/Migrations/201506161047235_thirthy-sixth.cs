namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirthysixth : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Activities", "AssigneeDetails");
            DropColumn("dbo.Activities", "Details");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Activities", "Details", c => c.String());
            AddColumn("dbo.Activities", "AssigneeDetails", c => c.String());
        }
    }
}
