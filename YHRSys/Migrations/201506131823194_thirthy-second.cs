namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirthysecond : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "AssigneeDetails", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "AssigneeDetails");
        }
    }
}
