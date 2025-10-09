using DataAccess_Layer.EF.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Interfaces
{
    public interface IPaymentRepo
    {
        bool Create(Payment s);
        List<Payment> Get();
        Payment Get(int id);
        bool Update(Payment s);
        bool Delete(int id);
        List<Payment> Get_By_Customer_Loan(int Customer_Loan_Id, int Customer_Id);
        List<Payment> Get_By_Customer(int Customer_Id);
    }
}
