namespace DataAccess_Layer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_New_Attributes_CustomerLoan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerLoans", "Next_Installment_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.CustomerLoans", "Next_Installment_Amount", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomerLoans", "Next_Installment_Amount");
            DropColumn("dbo.CustomerLoans", "Next_Installment_Date");
        }
    }
}
