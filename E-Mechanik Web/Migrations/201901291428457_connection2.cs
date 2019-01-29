namespace E_Mechanik_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class connection2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AvailableServices", "AvailableServiceCategory_Id", "dbo.AvailableServiceCategories");
            DropIndex("dbo.AvailableServices", new[] { "AvailableServiceCategory_Id" });
            RenameColumn(table: "dbo.AvailableServices", name: "AvailableServiceCategory_Id", newName: "AvailableServiceCategoryId");
            AlterColumn("dbo.AvailableServices", "AvailableServiceCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.AvailableServices", "AvailableServiceCategoryId");
            AddForeignKey("dbo.AvailableServices", "AvailableServiceCategoryId", "dbo.AvailableServiceCategories", "Id", cascadeDelete: true);
            DropColumn("dbo.AvailableServices", "ServiceCategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AvailableServices", "ServiceCategoryId", c => c.Int(nullable: false));
            DropForeignKey("dbo.AvailableServices", "AvailableServiceCategoryId", "dbo.AvailableServiceCategories");
            DropIndex("dbo.AvailableServices", new[] { "AvailableServiceCategoryId" });
            AlterColumn("dbo.AvailableServices", "AvailableServiceCategoryId", c => c.Int());
            RenameColumn(table: "dbo.AvailableServices", name: "AvailableServiceCategoryId", newName: "AvailableServiceCategory_Id");
            CreateIndex("dbo.AvailableServices", "AvailableServiceCategory_Id");
            AddForeignKey("dbo.AvailableServices", "AvailableServiceCategory_Id", "dbo.AvailableServiceCategories", "Id");
        }
    }
}
