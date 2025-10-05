namespace DataAccess_Layer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Minimum_Credit_For_Loan_Customer_Status_Loan_Status_End_Date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerLoans", "Status", c => c.String());
            AddColumn("dbo.CustomerLoans", "Loan_End_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customers", "Monthly_Income", c => c.Single(nullable: false));
            AddColumn("dbo.Customers", "Status", c => c.String());
            AddColumn("dbo.Loans", "Minimum_Credit_Score", c => c.Single(nullable: false));
            AddColumn("dbo.Payments", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "Type");
            DropColumn("dbo.Loans", "Minimum_Credit_Score");
            DropColumn("dbo.Customers", "Status");
            DropColumn("dbo.Customers", "Monthly_Income");
            DropColumn("dbo.CustomerLoans", "Loan_End_Date");
            DropColumn("dbo.CustomerLoans", "Status");
        }
    }
}
