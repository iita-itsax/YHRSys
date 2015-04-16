namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Third : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Varieties", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.Varieties", new[] { "userId" });
            AlterColumn("dbo.Varieties", "userId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Varieties", "userId");
            AddForeignKey("dbo.Varieties", "userId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Varieties", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.Varieties", new[] { "userId" });
            AlterColumn("dbo.Varieties", "userId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Varieties", "userId");
            AddForeignKey("dbo.Varieties", "userId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
