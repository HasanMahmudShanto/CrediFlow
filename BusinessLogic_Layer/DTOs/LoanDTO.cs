using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic_Layer.DTOs
{
    public class LoanDTO
    {
        public int Loan_Id { get; set; }
        public string Loan_Type { get; set; }
        public int Loan_Duration_Months { get; set; }
        public float Interest_Percentage { get; set; }
        public float Penalty_Amount { get; set; }
        public int Installment_Interval_Duration { get; set; }
    }
}
