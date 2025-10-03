namespace DataAccess_Layer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix_Customer_Loan_Id_Name : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Payments", name: "CustomerLoan_Id", newName: "Customer_Loan_Id");
            RenameIndex(table: "dbo.Payments", name: "IX_CustomerLoan_Id", newName: "IX_Customer_Loan_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Payments", name: "IX_Customer_Loan_Id", newName: "IX_CustomerLoan_Id");
            RenameColumn(table: "dbo.Payments", name: "Customer_Loan_Id", newName: "CustomerLoan_Id");
        }
    }
}
