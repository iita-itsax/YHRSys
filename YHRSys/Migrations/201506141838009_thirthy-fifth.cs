namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirthyfifth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "Details", c => c.String());
            AlterColumn("dbo.Activities", "AssigneeDetails", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Activities", "AssigneeDetails", c => c.String());
            DropColumn("dbo.Activities", "Details");
        }
    }
}
