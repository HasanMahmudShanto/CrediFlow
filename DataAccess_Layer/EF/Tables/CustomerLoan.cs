using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.EF.Tables
{
    public class CustomerLoan
    {
        [Key]
        public int Customer_Loan_Id { get; set; }
        [ForeignKey("Customer")]
        public int Customer_Id { get; set; }
        [ForeignKey("Loan")]
        public int Loan_Id { get; set; }
        public DateTime Loan_Taken_Date { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Loan Loan { get; set; }

    }
}
