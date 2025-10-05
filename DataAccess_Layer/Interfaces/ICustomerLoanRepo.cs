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
        CustomerLoan Update(CustomerLoan s);
        bool Delete(int id);
        bool Check_Existance_Of_Same_Loan(int customer_id, int loan_id);
        List<CustomerLoan> Get_All_Active_Loans();
        List<CustomerLoan> Get_By_Customer(int customer_id);

    }
}
