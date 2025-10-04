namespace DataAccess_Layer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Total_Paid_Amount_into_CustomerLoan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerLoans", "Total_Paid_Amount", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerLoans", "Total_Paid_Amount");
        }
    }
}
