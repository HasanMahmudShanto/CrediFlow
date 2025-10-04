using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic_Layer.DTOs
{
    public class CustomerLoanDTO
    {
        public int Customer_Loan_Id { get; set; }
        public int Customer_Id { get; set; }
        public int Loan_Id { get; set; }
        public DateTime Loan_Taken_Date { get; set; }
        public float Outstanding_Amount { get; set; }
        public DateTime Next_Installment_Date { get; set; }
        public float Next_Installment_Amount { get; set; }
        public float Total_Paid_Amount { get; set; }
        public LoanDTO LoanDTO { get; set; }
        public CustomerDTO CustomerDTO { get; set; }

    }
}
