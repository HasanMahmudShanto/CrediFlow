namespace DataAccess_Layer.Migrations
{
    using DataAccess_Layer.EF.Tables;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccess_Layer.EF.CrediFlowContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataAccess_Layer.EF.CrediFlowContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            context.Customers.AddOrUpdate(
                c => c.Customer_Id,
                new Customer { Customer_Id = 1, Name = "John Smith", Address = "123 Main St", Credit_Score = 720.5f, Gender = 1, Card_Number = "4111111111111111" },
                new Customer { Customer_Id = 2, Name = "Alice Brown", Address = "456 Oak Ave", Credit_Score = 650.0f, Gender = 2, Card_Number = "5500000000000004" },
                new Customer { Customer_Id = 3, Name = "Bob Johnson", Address = "789 Pine Rd", Credit_Score = 800.0f, Gender = 1, Card_Number = "340000000000009" }
            );

            // Seed Loans
            context.Loans.AddOrUpdate(
                l => l.Loan_Id,
                new Loan { Loan_Id = 1, Loan_Type = "Home Loan", Loan_Duration_Months = 240, Interest_Percentage = 6.5f, Penalty_Amount = 500.0f, Installment_Interval_Duration = 12 },
                new Loan { Loan_Id = 2, Loan_Type = "Car Loan", Loan_Duration_Months = 60, Interest_Percentage = 8.0f, Penalty_Amount = 200.0f, Installment_Interval_Duration = 6 },
                new Loan { Loan_Id = 3, Loan_Type = "Personal Loan", Loan_Duration_Months = 36, Interest_Percentage = 12.0f, Penalty_Amount = 100.0f, Installment_Interval_Duration = 1 }
            );

            // Seed CustomerLoans
            context.CustomerLoans.AddOrUpdate(
                cl => cl.Customer_Loan_Id,
                new CustomerLoan { Customer_Loan_Id = 1, Customer_Id = 1, Loan_Id = 1, Loan_Taken_Date = new DateTime(2023, 1, 15) },
                new CustomerLoan { Customer_Loan_Id = 2, Customer_Id = 2, Loan_Id = 2, Loan_Taken_Date = new DateTime(2024, 3, 10) },
                new CustomerLoan { Customer_Loan_Id = 3, Customer_Id = 3, Loan_Id = 3, Loan_Taken_Date = new DateTime(2025, 6, 20) }
            );
        }
    }
}
