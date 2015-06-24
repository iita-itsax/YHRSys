namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirthythird : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Activities", "AssigneeDetails");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Activities", "AssigneeDetails", c => c.String());
        }
    }
}
