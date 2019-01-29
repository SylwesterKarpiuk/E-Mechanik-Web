namespace E_Mechanik_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class connection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Services", "AvailableServiceCategories_Id", "dbo.AvailableServiceCategories");
            DropIndex("dbo.Services", new[] { "AvailableServiceCategories_Id" });
            DropColumn("dbo.Services", "AvailableServiceCategoryId");
            RenameColumn(table: "dbo.AvailableServices", name: "AvailableServiceCategories_Id", newName: "AvailableServiceCategory_Id");
            RenameColumn(table: "dbo.Services", name: "AvailableServiceCategories_Id", newName: "AvailableServiceCategoryId");
            RenameIndex(table: "dbo.AvailableServices", name: "IX_AvailableServiceCategories_Id", newName: "IX_AvailableServiceCategory_Id");
            AlterColumn("dbo.Services", "AvailableServiceCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Services", "AvailableServiceCategoryId");
            AddForeignKey("dbo.Services", "AvailableServiceCategoryId", "dbo.AvailableServiceCategories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "AvailableServiceCategoryId", "dbo.AvailableServiceCategories");
            DropIndex("dbo.Services", new[] { "AvailableServiceCategoryId" });
            AlterColumn("dbo.Services", "AvailableServiceCategoryId", c => c.Int());
            RenameIndex(table: "dbo.AvailableServices", name: "IX_AvailableServiceCategory_Id", newName: "IX_AvailableServiceCategories_Id");
            RenameColumn(table: "dbo.Services", name: "AvailableServiceCategoryId", newName: "AvailableServiceCategories_Id");
            RenameColumn(table: "dbo.AvailableServices", name: "AvailableServiceCategory_Id", newName: "AvailableServiceCategories_Id");
            AddColumn("dbo.Services", "AvailableServiceCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Services", "AvailableServiceCategories_Id");
            AddForeignKey("dbo.Services", "AvailableServiceCategories_Id", "dbo.AvailableServiceCategories", "Id");
        }
    }
}
