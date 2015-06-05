namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twentyeight : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteContents",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        caption = c.String(),
                        summary = c.String(),
                        fullArticle = c.String(),
                        status = c.Int(nullable: false),
                        userId = c.String(maxLength: 128),
                        createdDate = c.DateTime(),
                        updatedDate = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        createdBy = c.String(),
                        updatedBy = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.userId)
                .Index(t => t.userId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiteContents", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.SiteContents", new[] { "userId" });
            DropTable("dbo.SiteContents");
        }
    }
}
