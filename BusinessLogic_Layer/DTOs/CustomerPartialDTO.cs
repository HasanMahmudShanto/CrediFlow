using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic_Layer.DTOs
{
    public class CustomerPartialDTO
    {

        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public float Credit_Score { get; set; }
        public string Status { get; set; }

    }
}
