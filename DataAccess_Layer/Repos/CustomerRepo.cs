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
    public class CustomerRepo : ICustomerRepo
    {

        CrediFlowContext db;
        public CustomerRepo()
        {
            db = new CrediFlowContext();
        }
        public Customer Create(Customer Customer_data)
        {
            db.Customers.Add(Customer_data);
            db.SaveChanges();
            return Customer_data;
        }

        public bool Delete(int id)
        {
            var ex = db.Customers.Find(id);
            db.Customers.Remove(ex);
            db.SaveChanges();
            return true;
        }

        public List<Customer> Get()
        {
            return db.Customers.ToList();
        }

        public Customer Get(int id)
        {
            return db.Customers.Find(id);
        }

        public Customer Update(Customer Customer_Data)
        {
            var ex = db.Customers.Find(Customer_Data.Customer_Id);
            db.Entry(ex).CurrentValues.SetValues(Customer_Data);
            db.SaveChanges();
            return Customer_Data;
        }
    }
}
