namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fiftyfour : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "partnerId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "partnerId");
            AddForeignKey("dbo.AspNetUsers", "partnerId", "dbo.Partners", "partnerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "partnerId", "dbo.Partners");
            DropIndex("dbo.AspNetUsers", new[] { "partnerId" });
            DropColumn("dbo.AspNetUsers", "partnerId");
        }
    }
}
