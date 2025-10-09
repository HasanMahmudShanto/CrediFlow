namespace DataAccess_Layer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix_Attribute_names_Notification : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Notifications", name: "CustomerId", newName: "Customer_Id");
            RenameIndex(table: "dbo.Notifications", name: "IX_CustomerId", newName: "IX_Customer_Id");
            AddColumn("dbo.Notifications", "Is_Read", c => c.Boolean(nullable: false));
            DropColumn("dbo.Notifications", "IsRead");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "IsRead", c => c.Boolean(nullable: false));
            DropColumn("dbo.Notifications", "Is_Read");
            RenameIndex(table: "dbo.Notifications", name: "IX_Customer_Id", newName: "IX_CustomerId");
            RenameColumn(table: "dbo.Notifications", name: "Customer_Id", newName: "CustomerId");
        }
    }
}
