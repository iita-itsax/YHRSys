namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fortysixth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "status", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "status");
        }
    }
}
