namespace DataAccess_Layer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize_database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Customer_Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Credit_Score = c.Single(nullable: false),
                        Gender = c.Single(nullable: false),
                        Card_Number = c.String(),
                    })
                .PrimaryKey(t => t.Customer_Id);
            
            CreateTable(
                "dbo.CustomerLoans",
                c => new
                    {
                        Customer_Loan_Id = c.Int(nullable: false, identity: true),
                        Customer_Id = c.Int(nullable: false),
                        Loan_Id = c.Int(nullable: false),
                        Loan_Taken_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Customer_Loan_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .ForeignKey("dbo.Loans", t => t.Loan_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id)
                .Index(t => t.Loan_Id);
            
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        Loan_Id = c.Int(nullable: false, identity: true),
                        Loan_Type = c.String(),
                        Loan_Duration_Months = c.Int(nullable: false),
                        Interest_Percentage = c.Single(nullable: false),
                        Penalty_Amount = c.Single(nullable: false),
                        Installment_Interval_Duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Loan_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerLoans", "Loan_Id", "dbo.Loans");
            DropForeignKey("dbo.CustomerLoans", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.CustomerLoans", new[] { "Loan_Id" });
            DropIndex("dbo.CustomerLoans", new[] { "Customer_Id" });
            DropTable("dbo.Loans");
            DropTable("dbo.CustomerLoans");
            DropTable("dbo.Customers");
        }
    }
}
