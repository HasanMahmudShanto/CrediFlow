using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic_Layer.DTOs
{
    public class NotificationDTO
    {
        // Notification_Id is auto-generated 
        public int Notification_Id { get; set; }

        //Title is required. (From JS: if (!title))
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        //Message is required.
        [Required(ErrorMessage = "Message cannot be empty.")]
        public string Message { get; set; }

        public DateTime Date { get; set; }

        //Customer ID is required.
        [Required(ErrorMessage = "Please select a customer name.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid customer.")]
        public int Customer_Id { get; set; }

        public bool Is_Read { get; set; }

        // Note: CustomerDTO object itself does not need validation here as it's an included entity.
        public CustomerDTO CustomerDTO { get; set; }

    }
}