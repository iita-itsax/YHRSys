namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twentyfourth : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Reagents", "TotalReagentUsed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reagents", "TotalReagentUsed", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
