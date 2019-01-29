namespace E_Mechanik_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class carmodeldelete : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Cars");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        Brand = c.String(nullable: false),
                        ModelName = c.String(nullable: false),
                        Body = c.String(),
                        EngineCapacity = c.String(),
                        FuelType = c.String(),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
