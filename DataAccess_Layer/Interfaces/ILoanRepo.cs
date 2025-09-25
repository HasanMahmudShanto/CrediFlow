using DataAccess_Layer.EF.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Interfaces
{
    public interface ILoanRepo
    {

        bool Create(Loan s);
        List<Loan> Get();
        Loan Get(int id);
        bool Update(Loan s);
        bool Delete(int id);
        
    }
}
