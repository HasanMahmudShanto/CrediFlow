using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic_Layer.DTOs
{
    public class NotificationDTO
    {
        public int Notification_Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public bool IsRead { get; set; }
    }
}
