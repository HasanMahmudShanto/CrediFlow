namespace DataAccess_Layer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_Payment_Type : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Payments", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "Type", c => c.String());
        }
    }
}
