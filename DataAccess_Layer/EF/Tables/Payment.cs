using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.EF.Tables
{
    public class Payment
    {
        [Key]
        public int Payment_Id { get; set; }
        public DateTime Payment_Date { get; set; }
        [ForeignKey("Customer")]
        public int Customer_Id { get; set; }
        [ForeignKey("CustomerLoan")]
        public int Customer_Loan_Id { get; set; }
        public float Amount { get; set; }


        public virtual CustomerLoan CustomerLoan { get; set; }
        public virtual Customer Customer { get; set; }

    }
}
