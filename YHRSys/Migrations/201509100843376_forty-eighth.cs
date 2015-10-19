namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fortyeighth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "createdDate", c => c.DateTime());
            AddColumn("dbo.OrderDetails", "updatedDate", c => c.DateTime());
            AddColumn("dbo.OrderDetails", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.OrderDetails", "createdBy", c => c.String());
            AddColumn("dbo.OrderDetails", "updatedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "updatedBy");
            DropColumn("dbo.OrderDetails", "createdBy");
            DropColumn("dbo.OrderDetails", "Timestamp");
            DropColumn("dbo.OrderDetails", "updatedDate");
            DropColumn("dbo.OrderDetails", "createdDate");
        }
    }
}
