using DataAccess_Layer.EF.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.EF
{
    public class CrediFlowContext : DbContext
    {

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerLoan> CustomerLoans { get; set; }
        public DbSet<Loan> Loans { get; set; }

    }
}
