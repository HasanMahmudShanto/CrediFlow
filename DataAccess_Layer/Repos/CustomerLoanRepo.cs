using DataAccess_Layer.EF;
using DataAccess_Layer.EF.Tables;
using DataAccess_Layer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Repos
{
    internal class CustomerLoanRepo : ICustomerLoanRepo
    {
        CrediFlowContext db;
        public CustomerLoanRepo() 
        { 
            db = new CrediFlowContext();
        }
        public CustomerLoan Create(CustomerLoan CustomerLoan_Data)
        {
            db.CustomerLoans.Add(CustomerLoan_Data);
            db.SaveChanges();
            return CustomerLoan_Data;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<CustomerLoan> Get()
        {
            throw new NotImplementedException();
        }

        public CustomerLoan Get(int id)
        {
            return db.CustomerLoans.Find(id);
        }

        public CustomerLoan Update(CustomerLoan CustomerLoan_Data)
        {
            var Data = db.CustomerLoans.Find(CustomerLoan_Data.Customer_Loan_Id);
            if (Data != null)
            {
                Data.Outstanding_Amount = CustomerLoan_Data.Outstanding_Amount;
                db.SaveChanges();
            }
            return Data;
        }
    }
}
