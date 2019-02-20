namespace E_Mechanik_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mechanicProfileFix4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MechanicProfiles", "MechanicName", c => c.String(nullable: false));
            AlterColumn("dbo.MechanicProfiles", "CompanyName", c => c.String(nullable: false));
            AlterColumn("dbo.MechanicProfiles", "City", c => c.String(nullable: false));
            AlterColumn("dbo.MechanicProfiles", "Address", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MechanicProfiles", "Address", c => c.String());
            AlterColumn("dbo.MechanicProfiles", "City", c => c.String());
            AlterColumn("dbo.MechanicProfiles", "CompanyName", c => c.String());
            AlterColumn("dbo.MechanicProfiles", "MechanicName", c => c.String());
        }
    }
}
