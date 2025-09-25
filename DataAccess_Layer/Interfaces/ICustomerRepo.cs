using DataAccess_Layer.EF.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Interfaces
{
    public interface ICustomerRepo
    {

        bool Create(Customer s);
        List<Customer> Get();
        Customer Get(int id);
        bool Update(Customer s);
        bool Delete(int id);

    }
}
