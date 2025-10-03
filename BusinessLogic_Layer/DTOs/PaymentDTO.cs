using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic_Layer.DTOs
{
    public class PaymentDTO
    {
        public int Payment_Id { get; set; }
        public DateTime Payment_Date { get; set; }
        public int Customer_Id { get; set; }
        public int Customer_Loan_Id { get; set; }
        public float Amount { get; set; }
    }
}
