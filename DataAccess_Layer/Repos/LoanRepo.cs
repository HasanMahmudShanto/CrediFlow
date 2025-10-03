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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
