namespace DataAccess_Layer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Payment_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Payment_Id = c.Int(nullable: false, identity: true),
                        Payment_Date = c.DateTime(nullable: false),
                        Customer_Id = c.Int(nullable: false),
                        CustomerLoan_Id = c.Int(nullable: false),
                        Amount = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Payment_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.CustomerLoans", t => t.CustomerLoan_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id)
                .Index(t => t.CustomerLoan_Id);
            
            AddColumn("dbo.CustomerLoans", "Outstanding_Amount", c => c.Single(nullable: false));
            AddColumn("dbo.Loans", "Installment_Amount", c => c.Single(nullable: false));
            AddColumn("dbo.Loans", "Loan_Amount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "CustomerLoan_Id", "dbo.CustomerLoans");
            DropForeignKey("dbo.Payments", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Payments", new[] { "CustomerLoan_Id" });
            DropIndex("dbo.Payments", new[] { "Customer_Id" });
            DropColumn("dbo.Loans", "Loan_Amount");
            DropColumn("dbo.Loans", "Installment_Amount");
            DropColumn("dbo.CustomerLoans", "Outstanding_Amount");
            DropTable("dbo.Payments");
        }
    }
}
