using BusinessLogic_Layer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic_Layer.Service
{
    public class ReminderService
    {
        public static void Get_Reminders()
        {
            var All_Active_Loans = CustomerLoanService.Get_All_Active_Loans();

            DateTime Target_Date_7 = DateTime.Today.AddDays(7);
            DateTime Target_Date_3 = DateTime.Today.AddDays(3);
            DateTime Target_Date_0 = DateTime.Today;

            foreach(var loan in All_Active_Loans)
            {
                if (loan.Next_Installment_Date.Date == Target_Date_7)
                {
                    var notification = new NotificationDTO
                    {
                        CustomerId = loan.Customer_Id,
                        Date = DateTime.Now,
                        IsRead = false,
                        Title = "Upcoming Installment Due",
                        Message = $"Your next loan installment is due on {loan.Next_Installment_Date:yyyy-MM-dd}."
                    };
                    NotificationService.Create(notification);
                }
                if (loan.Next_Installment_Date.Date == Target_Date_3)
                {
                    var notification = new NotificationDTO
                    {
                        CustomerId = loan.Customer_Id,
                        Date = DateTime.Now,
                        IsRead = false,
                        Title = "Upcoming Installment Due",
                        Message = $"Your next loan installment is due on {loan.Next_Installment_Date:yyyy-MM-dd}."
                    };
                    NotificationService.Create(notification);
                }
                if (loan.Next_Installment_Date.Date == Target_Date_0)
                {
                    var notification = new NotificationDTO
                    {
                        CustomerId = loan.Customer_Id,
                        Date = DateTime.Now,
                        IsRead = false,
                        Title = "Upcoming Installment Due",
                        Message = $"Your next loan installment is due on Today."
                    };
                    NotificationService.Create(notification);
                }
            }
        }
    }
}
