namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Token", c => c.String());
            AddColumn("dbo.AspNetUsers", "TokenExpired", c => c.Boolean());
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String());
            DropColumn("dbo.AspNetUsers", "TokenExpired");
            DropColumn("dbo.AspNetUsers", "Token");
        }
    }
}
