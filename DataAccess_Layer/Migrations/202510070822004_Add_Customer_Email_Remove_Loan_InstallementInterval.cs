namespace DataAccess_Layer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Customer_Email_Remove_Loan_InstallementInterval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Email", c => c.String());
            DropColumn("dbo.Loans", "Installment_Interval_Duration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Loans", "Installment_Interval_Duration", c => c.Int(nullable: false));
            DropColumn("dbo.Customers", "Email");
        }
    }
}
