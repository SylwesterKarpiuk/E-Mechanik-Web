namespace E_Mechanik_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MechanicProfileFix3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MechanicProfiles", "position_Lat", c => c.String());
            AlterColumn("dbo.MechanicProfiles", "position_Lon", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MechanicProfiles", "position_Lon", c => c.Double(nullable: false));
            AlterColumn("dbo.MechanicProfiles", "position_Lat", c => c.Double(nullable: false));
        }
    }
}
