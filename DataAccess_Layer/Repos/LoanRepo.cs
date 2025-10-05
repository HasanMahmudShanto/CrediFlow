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
    public class LoanRepo : ILoanRepo
    {
        CrediFlowContext db;
        public LoanRepo()
        {
            db = new CrediFlowContext();
        }
        public Loan Create(Loan Loan_Data)
        {
            db.Loans.Add(Loan_Data);
            db.SaveChanges();
            return Loan_Data;
        }

        public bool Delete(int id)
        {
            var ex = db.Loans.Find(id);
            db.Loans.Remove(ex);
            db.SaveChanges();
            return true;
        }

        public List<Loan> Get()
        {
            return db.Loans.ToList();
        }

        public Loan Get(int id)
        {
            var Data = db.Loans.Find(id);
            return Data;
        }

        public bool Update(Loan s)
        {
            var ex = db.Loans.Find(s.Loan_Id);
            db.Entry(ex).CurrentValues.SetValues(s);
            db.SaveChanges();
            return true;
        }
        public List<Loan> Get_Eligible_Loans(float credit_score)
        {
            var Eligible_Loans = db.Loans.Where(loan => loan.Minimum_Credit_Score <= credit_score).ToList();
            return Eligible_Loans;
        }
    }
}
