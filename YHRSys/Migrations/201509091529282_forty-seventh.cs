namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fortyseventh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "createdDate", c => c.DateTime());
            AddColumn("dbo.Orders", "updatedDate", c => c.DateTime());
            AddColumn("dbo.Orders", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Orders", "createdBy", c => c.String());
            AddColumn("dbo.Orders", "updatedBy", c => c.String());
            AlterColumn("dbo.Orders", "status", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "status", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "updatedBy");
            DropColumn("dbo.Orders", "createdBy");
            DropColumn("dbo.Orders", "Timestamp");
            DropColumn("dbo.Orders", "updatedDate");
            DropColumn("dbo.Orders", "createdDate");
        }
    }
}
