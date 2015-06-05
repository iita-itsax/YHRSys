namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twentynineth : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SiteContents", "caption", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SiteContents", "caption", c => c.String());
        }
    }
}
