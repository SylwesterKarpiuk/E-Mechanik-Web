namespace E_Mechanik_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mechanicProfileImageAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MechanicProfiles", "ImagePatch", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MechanicProfiles", "ImagePatch");
        }
    }
}
