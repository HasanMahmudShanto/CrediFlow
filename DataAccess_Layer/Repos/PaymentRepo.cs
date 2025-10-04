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
            var ex = db.Payments.Find(id);
            db.Payments.Remove(ex);
            db.SaveChanges();
            return true;
        }

        public List<Payment> Get()
        {
            return db.Payments.ToList();
        }

        public Payment Get(int id)
        {
            return db.Payments.Find(id);
        }

        public bool Update(Payment s)
        {
            var Data = db.Payments.Find(s.Payment_Id);
            db.Entry(Data).CurrentValues.SetValues(s);
            db.SaveChanges();
            return true;
        }
    }
}
