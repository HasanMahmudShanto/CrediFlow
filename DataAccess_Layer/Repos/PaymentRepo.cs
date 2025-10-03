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
    public class PaymentRepo : IPaymentRepo
    {
        CrediFlowContext db;
        public PaymentRepo()
        {
            db = new CrediFlowContext();
        }
        public bool Create(Payment s)
        {
            db.Payments.Add(s);
            db.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Payment> Get()
        {
            throw new NotImplementedException();
        }

        public Payment Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Payment s)
        {
            throw new NotImplementedException();
        }
    }
}
