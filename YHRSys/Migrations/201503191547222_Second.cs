namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Varieties", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.Varieties", new[] { "userId" });
            CreateIndex("dbo.Varieties", "userId");
            AddForeignKey("dbo.Varieties", "userId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Varieties", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.Varieties", new[] { "userId" });
            CreateIndex("dbo.Varieties", "userId");
            AddForeignKey("dbo.Varieties", "userId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
