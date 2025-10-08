using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.EF.Tables
{
    public class Customer
    {
        [Key]
        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public float Credit_Score { get; set; }
        public float Gender { get; set; }
        public string Card_Number { get; set; }
        public float Monthly_Income { get; set; }
        public string Status { get; set; }

    }
}
