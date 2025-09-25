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
        public bool Create(Customer s)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Customer> Get()
        {
            return db.Customers.ToList();
        }

        public Customer Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Customer s)
        {
            throw new NotImplementedException();
        }
    }
}
