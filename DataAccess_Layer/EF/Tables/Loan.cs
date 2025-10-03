using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.EF.Tables
{
    public class Loan
    {
        [Key]
        public int Loan_Id { get; set; }
        public string Loan_Type { get; set; }
        public int Loan_Duration_Months { get; set; }
        public float Interest_Percentage { get; set; }
        public float Penalty_Amount { get; set; }
        public int Installment_Interval_Duration { get; set; }
        public float Installment_Amount { get; set; }
        public int Loan_Amount {  get; set; }

    }
}
