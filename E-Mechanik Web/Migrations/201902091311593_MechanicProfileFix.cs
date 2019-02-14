namespace E_Mechanik_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MechanicProfileFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MechanicProfiles", "position_Lat", c => c.String());
            AddColumn("dbo.MechanicProfiles", "position_Lon", c => c.String());
            DropColumn("dbo.MechanicProfiles", "Country");
            DropColumn("dbo.MechanicProfiles", "PostalCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MechanicProfiles", "PostalCode", c => c.String());
            AddColumn("dbo.MechanicProfiles", "Country", c => c.String());
            DropColumn("dbo.MechanicProfiles", "position_Lon");
            DropColumn("dbo.MechanicProfiles", "position_Lat");
        }
    }
}
