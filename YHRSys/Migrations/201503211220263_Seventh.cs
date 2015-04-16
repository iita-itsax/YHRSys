namespace YHRSys.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seventh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PartnerContacts",
                c => new
                    {
                        contactId = c.Int(nullable: false, identity: true),
                        partnerId = c.Int(nullable: false),
                        firstName = c.String(nullable: false),
                        otherNames = c.String(),
                        lastName = c.String(nullable: false),
                        phoneNumber = c.String(),
                        emailAddress = c.String(),
                        contactAddress = c.String(),
                        webAddress = c.String(),
                        geoLongitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        geoLatitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        createdDate = c.DateTime(),
                        updatedDate = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        createdBy = c.String(),
                        updatedBy = c.String(),
                    })
                .PrimaryKey(t => t.contactId);
            
            DropTable("dbo.PartnerContactPersons");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PartnerContactPersons",
                c => new
                    {
                        contactId = c.Int(nullable: false, identity: true),
                        partnerId = c.Int(nullable: false),
                        firstName = c.String(nullable: false),
                        otherNames = c.String(),
                        lastName = c.String(nullable: false),
                        phoneNumber = c.String(),
                        emailAddress = c.String(),
                        contactAddress = c.String(),
                        webAddress = c.String(),
                        geoLongitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        geoLatitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        createdDate = c.DateTime(),
                        updatedDate = c.DateTime(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        createdBy = c.String(),
                        updatedBy = c.String(),
                    })
                .PrimaryKey(t => t.contactId);
            
            DropTable("dbo.PartnerContacts");
        }
    }
}
