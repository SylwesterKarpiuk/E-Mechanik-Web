namespace E_Mechanik_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MechanicProfiles", "PhoneNumber");
            DropColumn("dbo.MechanicProfiles", "position_Lat");
            DropColumn("dbo.MechanicProfiles", "position_Lon");
            DropTable("dbo.ClientProfiles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ClientProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        CarBrand = c.String(),
                        CarModel = c.String(),
                        BodyType = c.String(),
                        EngineCapacity = c.String(),
                        GasType = c.String(),
                        LastTechnicalExamination = c.String(),
                        InsuranceEndDate = c.String(),
                        Country = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.MechanicProfiles", "position_Lon", c => c.String());
            AddColumn("dbo.MechanicProfiles", "position_Lat", c => c.String());
            AddColumn("dbo.MechanicProfiles", "PhoneNumber", c => c.String());
        }
    }
}
