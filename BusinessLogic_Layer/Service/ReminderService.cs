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
            try
            {
                var All_Active_Loans = CustomerLoanService.Get_All_Active_Loans();

                DateTime Target_Date_7 = DateTime.Today.AddDays(7);
                DateTime Target_Date_3 = DateTime.Today.AddDays(3);
                DateTime Target_Date_0 = DateTime.Today;

                foreach (var loan in All_Active_Loans)
                {
                    string formattedDate = loan.Next_Installment_Date.ToString("MMMM dd, yyyy");

                    // 7 days before due date
                    if (loan.Next_Installment_Date.Date == Target_Date_7)
                    {
                        var notification = new NotificationDTO
                        {
                            Customer_Id = loan.Customer_Id,
                            Date = DateTime.Now,
                            Is_Read = false,
                            Title = "Installment Due in 7 Days",
                            Message = $"Dear {loan.CustomerDTO.Name},\n\n" +
                                      $"This is a friendly reminder that your next loan installment of {loan.Next_Installment_Amount} is due on {formattedDate}.\n" +
                                      "Please ensure timely payment to avoid any late fees.\n\n" +
                                      "Thank you for choosing CrediFlow.",
                            CustomerDTO = loan.CustomerDTO
                        };
                        NotificationService.Create(notification);
                    }

                    // 3 days before due date
                    if (loan.Next_Installment_Date.Date == Target_Date_3)
                    {
                        var notification = new NotificationDTO
                        {
                            Customer_Id = loan.Customer_Id,
                            Date = DateTime.Now,
                            Is_Read = false,
                            Title = "Installment Due in 3 Days",
                            Message = $"Dear {loan.CustomerDTO.Name},\n\n" +
                                      $"Your next loan installment of {loan.Next_Installment_Amount} is due on {formattedDate}.\n" +
                                      "Please make your payment on or before the due date to maintain a good credit standing.\n\n" +
                                      "Best regards,\nThe CrediFlow Team",
                            CustomerDTO = loan.CustomerDTO
                        };
                        NotificationService.Create(notification);
                    }

                    // On due date
                    if (loan.Next_Installment_Date.Date == Target_Date_0)
                    {
                        var notification = new NotificationDTO
                        {
                            Customer_Id = loan.Customer_Id,
                            Date = DateTime.Now,
                            Is_Read = false,
                            Title = "Installment Due Today",
                            Message = $"Dear {loan.CustomerDTO.Name},\n\n" +
                                      $"Your loan installment of {loan.Next_Installment_Amount} is due today ({formattedDate}).\n" +
                                      "Please make your payment as soon as possible to avoid any penalties.\n\n" +
                                      "Thank you for being a valued CrediFlow customer.",
                            CustomerDTO = loan.CustomerDTO
                        };
                        NotificationService.Create(notification);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in Get_Reminders(): {0}", ex.ToString());
            }

        }
    }
}
