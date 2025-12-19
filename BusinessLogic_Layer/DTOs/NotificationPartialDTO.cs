using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic_Layer.DTOs
{
    public class NotificationPartialDTO
    {
        public int Notification_Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public int Customer_Id { get; set; }
        public bool Is_Read { get; set; }
        public CustomerDTO CustomerDTO { get; set; }

    }
}
