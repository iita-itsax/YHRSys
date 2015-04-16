namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nineteen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PartnerContacts", "gender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PartnerContacts", "gender");
        }
    }
}
