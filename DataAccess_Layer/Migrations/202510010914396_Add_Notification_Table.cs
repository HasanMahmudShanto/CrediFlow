namespace DataAccess_Layer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Notification_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Notification_Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Message = c.String(),
                        Date = c.DateTime(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Notification_Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Notifications", new[] { "CustomerId" });
            DropTable("dbo.Notifications");
        }
    }
}
