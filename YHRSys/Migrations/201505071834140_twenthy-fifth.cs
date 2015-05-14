namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twenthyfifth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reagents", "type", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reagents", "type");
        }
    }
}
