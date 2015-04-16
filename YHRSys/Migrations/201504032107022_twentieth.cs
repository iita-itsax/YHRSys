namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twentieth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reagents", "reOrderLevel", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reagents", "reOrderLevel");
        }
    }
}
