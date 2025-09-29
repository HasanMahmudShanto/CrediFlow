using DataAccess_Layer.EF.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Interfaces
{
    public interface ICustomerLoanRepo
    {
        CustomerLoan Create(CustomerLoan s);
        List<CustomerLoan> Get();
        CustomerLoan Get(int id);
        bool Update(CustomerLoan s);
        bool Delete(int id);
    }
}
