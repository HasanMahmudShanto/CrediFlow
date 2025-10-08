namespace DataAccess_Layer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Penalty_Percentage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "Penalty_Percentage", c => c.Single(nullable: false));
            DropColumn("dbo.Loans", "Penalty_Amount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Loans", "Penalty_Amount", c => c.Single(nullable: false));
            DropColumn("dbo.Loans", "Penalty_Percentage");
        }
    }
}
