using AutoMapper;
using BusinessLogic_Layer.DTOs;
using DataAccess_Layer.EF.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic_Layer.Service
{
    public class LateFeeService
    {
        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Customer, CustomerDTO>().ReverseMap();
                cfg.CreateMap<CustomerLoan, CustomerLoanDTO>().ReverseMap();
                cfg.CreateMap<Loan, LoanDTO>().ReverseMap();
                cfg.CreateMap<Notification, NotificationDTO>().ReverseMap();
                cfg.CreateMap<Payment, PaymentDTO>().ReverseMap();
            });
            return new Mapper(config);
        }



        public static void Apply_Late_Fee()
        {
            try
            {
                List<CustomerLoanDTO> CustomerLoans = CustomerLoanService.Get();
                foreach (var loan in CustomerLoans)
                {
                    if (loan.Status == "Active" && DateTime.Now > loan.Next_Installment_Date && loan.CustomerDTO.Status != "Terminated") //Late
                    {
                        if (loan.CustomerDTO.Status == "Restricted")
                        {
                            PaymentService.Terminate_Customer(loan.CustomerDTO, loan);
                        }
                        //Applying late fee
                        float Late_Fee = (loan.Next_Installment_Amount * (loan.LoanDTO.Penalty_Percentage / 100));
                        loan.Next_Installment_Amount += Late_Fee;
                        loan.Outstanding_Amount += Late_Fee;
                        CustomerLoanService.Update(loan);
                        //Creating notification
                        NotificationDTO notification = new NotificationDTO
                        {
                            Customer_Id = loan.Customer_Id,
                            Message = $"A late fee of {Late_Fee} has been applied to your {loan.LoanDTO.Loan_Type} with ID:{loan.Customer_Loan_Id} with  due to a missed installment payment. Please make the payment at your earliest convenience to avoid further penalties.",
                            Date = DateTime.Now,
                            Is_Read = false,
                            CustomerDTO = loan.CustomerDTO,
                        };
                        NotificationService.Create(notification);

                        //deducting credit score
                        loan.CustomerDTO.Credit_Score -= 25;
                        CustomerService.Update(loan.CustomerDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in Apply_Late_Fee(): {0}", ex.ToString());
                Console.WriteLine($"Failed to apply late fee: {ex.Message}");

            }
        }
    }
}
