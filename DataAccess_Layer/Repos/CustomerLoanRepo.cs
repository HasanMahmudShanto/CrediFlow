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
            var ex = db.CustomerLoans.Find(id);
            db.CustomerLoans.Remove(ex);
            db.SaveChanges();
            return true;
        }

        public List<CustomerLoan> Get()
        {
            return db.CustomerLoans.ToList();
        }

        public CustomerLoan Get(int id)
        {
            return db.CustomerLoans.Find(id);
        }

        public CustomerLoan Update(CustomerLoan CustomerLoan_Data)
        {
            var Data = db.CustomerLoans.Find(CustomerLoan_Data.Customer_Loan_Id);
            db.Entry(Data).CurrentValues.SetValues(CustomerLoan_Data);
            db.SaveChanges();
            return Data;
        }
        public bool Check_Existance_Of_Same_Loan(int customer_id, int loan_id)
        {
            var Data = db.CustomerLoans.Where(cl => cl.Customer_Id == customer_id && cl.Loan_Id == loan_id).FirstOrDefault();
            if (Data == null) return false;
            return true;
        }
        public List<CustomerLoan> Get_By_Customer(int customer_id)
        {
            return db.CustomerLoans.Where(cl => cl.Customer_Id == customer_id).ToList();
        }
    }
}
