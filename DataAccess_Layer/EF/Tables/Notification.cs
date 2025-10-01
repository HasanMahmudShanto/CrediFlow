using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.EF.Tables
{
    public class Notification
    {
        [Key]
        public int Notification_Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public bool IsRead { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
