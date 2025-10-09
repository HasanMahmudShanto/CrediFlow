using AutoMapper;
using BusinessLogic_Layer.DTOs;
using DataAccess_Layer;
using DataAccess_Layer.EF.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic_Layer.Service
{
    public class PaymentService
    {
        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Payment, PaymentDTO>().ReverseMap();
                cfg.CreateMap<CustomerLoan, CustomerLoanDTO>().ReverseMap();
                cfg.CreateMap<Loan, LoanDTO>().ReverseMap();
                cfg.CreateMap<Customer, CustomerDTO>().ReverseMap();
            });
            return new Mapper(config);
        }

        
        public static bool isValidPayment(PaymentDTO PaymentDTO_Data, CustomerDTO CustomerDTO_Data, CustomerLoanDTO CustomerLoanDTO_Data, LoanDTO LoanDTO_Data)
        {
            //Checking if related data exists
            if (CustomerLoanDTO_Data == null || CustomerDTO_Data == null || LoanDTO_Data == null)
                return false;
            //Checking if the payment amount is valid
            else if (CustomerLoanDTO_Data.Outstanding_Amount < PaymentDTO_Data.Amount || CustomerLoanDTO_Data.Outstanding_Amount == 0)
                return false;
            //Checking if the loan is already closed or customer is terminated
            if (CustomerDTO_Data.Status == "Terminated" || CustomerLoanDTO_Data.Status == "Closed")
                return false;
            //Ensuring the payment is being made by the rightful customer
            if (PaymentDTO_Data.Customer_Id != CustomerLoanDTO_Data.Customer_Id)
                return false;

            return true;
        }

        public static void Terminate_Customer(CustomerDTO CustomerDTO_Data, CustomerLoanDTO CustomerLoanDTO_Data)
        {
            CustomerDTO_Data.Status = "Terminated"; // If restricted, then terminate the customer
            CustomerDTO_Data.Credit_Score = 0; // Reset credit score upon termination
            //populating customer loan data
            CustomerLoanDTO_Data.Status = "Closed";
            CustomerLoanDTO_Data.Next_Installment_Amount = 0.00f;
            // No next installment date if loan is fully paid so we put it max as in maxValue means no more active
            CustomerLoanDTO_Data.Next_Installment_Date = DateTime.MaxValue;
            CustomerLoanDTO_Data.Total_Paid_Amount = 0;
            CustomerLoanDTO_Data.Loan_End_Date = DateTime.Now;
            //Updating CustomerLoan in DB and returning updated data
            CustomerLoanDTO Data = GetMapper().Map<CustomerLoanDTO>(DataAccessFactory.CustomerLoanData().Update(GetMapper().Map<CustomerLoan>(CustomerLoanDTO_Data)));
            Data.CustomerDTO = CustomerDTO_Data;
            Data.LoanDTO = LoanService.Get(CustomerLoanDTO_Data.Loan_Id);

            //Creating Notification for the customer
            var NotificationDTO_Data = new NotificationDTO
            {
                Customer_Id = CustomerDTO_Data.Customer_Id,
                Title = "Account Terminated",
                Message = $"Dear {CustomerDTO_Data.Name},\n\n" +
                "We regret to inform you that your account has been terminated due to non-payment of your loan installments and low credit score. " +
                "Despite previous notifications, we have not received the required payments.\n\n" +
                "If you believe this is a mistake or wish to discuss your account, please contact our support team immediately.\n\n" +
                "Best regards,\nThe CrediFlow Team",
                CustomerDTO = CustomerDTO_Data
            };
            bool Notification_Data = NotificationService.Create(NotificationDTO_Data);



            //Saving updated customer data in DB
            CustomerDTO Updated_Customer_Data = CustomerService.Update(CustomerDTO_Data);
        }

        public static CustomerLoanDTO LoanReturn(PaymentDTO PaymentDTO_Data)
        {
            //Getting related data
            CustomerDTO CustomerDTO_Data = CustomerService.Get(PaymentDTO_Data.Customer_Id);
            CustomerLoanDTO CustomerLoanDTO_Data = CustomerLoanService.Get(PaymentDTO_Data.Customer_Loan_Id);
            LoanDTO LoanDTO_Data = LoanService.Get(CustomerLoanDTO_Data.Loan_Id);

            //Null checks and validations
            if (!isValidPayment(PaymentDTO_Data, CustomerDTO_Data, CustomerLoanDTO_Data, LoanDTO_Data))
                return null;
     

            //Processing payment
            PaymentDTO_Data.Payment_Date = DateTime.Now;
            PaymentDTO_Data.CustomerDTO = CustomerDTO_Data;
            PaymentDTO_Data.CustomerLoanDTO = CustomerLoanDTO_Data; 
            PaymentDTO_Data.Amount = CustomerLoanDTO_Data.Next_Installment_Amount;

           
            
            if(CustomerLoanDTO_Data.Next_Installment_Date - DateTime.Now >= TimeSpan.Zero) // On-time payment
            {
                if(CustomerDTO_Data.Status == "Restricted") //Re-activating restricted customers
                {
                    CustomerDTO_Data.Credit_Score = 300; // Reset credit score upon re-activation
                    CustomerDTO_Data.Status = "Poor";
                }
                else // Normal Monthly Installment Payment
                {
                    CustomerDTO_Data.Credit_Score += 20; // Increase credit score for on-time payment
                }
            }
            //Ensuring credit score remains within bounds
            if (CustomerDTO_Data.Credit_Score > 900) CustomerDTO_Data.Credit_Score = 900;
            else if (CustomerDTO_Data.Credit_Score < 0) CustomerDTO_Data.Credit_Score = 0;


            //Updating CustomerLoanDTO_Data

            CustomerLoanDTO_Data.Outstanding_Amount -= PaymentDTO_Data.Amount;

            //If there are some decimals left due to float calculations,
            //we consider the loan fully paid if outstanding amount is less than 1 currency unit
            if (CustomerLoanDTO_Data.Outstanding_Amount < 1.00f) 
                CustomerLoanDTO_Data.Outstanding_Amount = 0.00f;


            //If loan is fully paid, closing it
            if (CustomerLoanDTO_Data.Outstanding_Amount == 0)
            {   
                CustomerLoanDTO_Data.Status = "Closed";
                CustomerLoanDTO_Data.Next_Installment_Amount = 0.00f;

                // No next installment date if loan is fully paid so we put it max as in maxValue means no more active
                CustomerLoanDTO_Data.Next_Installment_Date = DateTime.MaxValue; 

                CustomerLoanDTO_Data.Total_Paid_Amount += PaymentDTO_Data.Amount;
                CustomerLoanDTO_Data.Loan_End_Date = DateTime.Now;
            }
            else //If loan is not fully paid, setting up next installment date and amount
            {
                CustomerLoanDTO_Data.Next_Installment_Date = CustomerLoanDTO_Data.Next_Installment_Date.AddMonths(1);
                CustomerLoanDTO_Data.Next_Installment_Amount = CustomerLoanDTO_Data.LoanDTO.Installment_Amount;
                CustomerLoanDTO_Data.Total_Paid_Amount += PaymentDTO_Data.Amount;
            }

            //Saving payment in DB
            var Payment_Data = DataAccessFactory.PaymentData().Create(GetMapper().Map<Payment>(PaymentDTO_Data));
            //Saving updated customer data in DB
            CustomerDTO Customer_Data = CustomerService.Update(CustomerDTO_Data);

            //Updating CustomerLoan in DB and returning updated data
            if (Payment_Data)
            {
                
                CustomerLoanDTO Data = GetMapper().Map<CustomerLoanDTO>(DataAccessFactory.CustomerLoanData().Update(GetMapper().Map<CustomerLoan>(CustomerLoanDTO_Data)));
                Data.CustomerDTO = CustomerDTO_Data;
                Data.LoanDTO = LoanDTO_Data;

                //Creating Notification for the customer
                var NotificationDTO_Data = new NotificationDTO
                {
                    Customer_Id = CustomerDTO_Data.Customer_Id,
                    Title = "Payment Received",
                    Message = $"Dear {CustomerDTO_Data.Name},\n\n" +
                    "We’re pleased to confirm that your recent payment has been received successfully. Here are the details of your transaction:\n\n" +
                    $"Payment Amount: {PaymentDTO_Data.Amount:C}\n" +
                    $"Payment Date: {PaymentDTO_Data.Payment_Date:MMMM dd, yyyy}\n" +
                    $"Loan Type: {Data.LoanDTO.Loan_Type}\n" +
                    $"Next Installment Date: {(Data.Status == "Closed" ? "N/A — Loan Fully Paid" : Data.Next_Installment_Date.ToString("MMMM dd, yyyy"))}\n" +
                    $"Next Installment Amount: {(Data.Status == "Closed" ? "N/A — Loan Fully Paid" : Data.Next_Installment_Amount.ToString("C"))}\n" +
                    $"Outstanding Amount: {Data.Outstanding_Amount:C}\n\n" +
                    (Data.Status == "Closed"
                        ? "Congratulations! Your loan has been fully paid off. Thank you for your commitment and trust in CrediFlow.\n\n"
                        : "Thank you for your timely payment. We appreciate your continued trust in CrediFlow.\n\n") +
                        "Best regards,\nThe CrediFlow Team",
                    CustomerDTO = CustomerDTO_Data
                };
                
                bool Notification_Data = NotificationService.Create(NotificationDTO_Data);

                return Data;
            }
            return null;
        }

        public static PaymentDTO Get(int id)
        {
            PaymentDTO PaymentDTO_Data = GetMapper().Map<PaymentDTO>(DataAccessFactory.PaymentData().Get(id));
            PaymentDTO_Data.CustomerDTO = CustomerService.Get(PaymentDTO_Data.Customer_Id);
            PaymentDTO_Data.CustomerLoanDTO = CustomerLoanService.Get(PaymentDTO_Data.Customer_Loan_Id);
            return PaymentDTO_Data;
        }
        public static List<PaymentDTO> Get()
        {
            List<PaymentDTO> PaymentDTO_Data = GetMapper().Map<List<PaymentDTO>>(DataAccessFactory.PaymentData().Get());
            foreach(var item in PaymentDTO_Data)
            {
                item.CustomerDTO = CustomerService.Get(item.Customer_Id);
                item.CustomerLoanDTO = CustomerLoanService.Get(item.Customer_Loan_Id);
            }
            return PaymentDTO_Data;
        }
        public static PaymentDTO Create(PaymentDTO PaymentDTO_Data)
        {
            var Data = DataAccessFactory.PaymentData().Create(GetMapper().Map<Payment>(PaymentDTO_Data));
            return GetMapper().Map<PaymentDTO>(Data);
        }

        public static PaymentDTO Merge_Update(PaymentDTO PaymentDTO_Data, PaymentDTO Prev_Data)
        {
            if(PaymentDTO_Data.Customer_Id == 0) PaymentDTO_Data.Customer_Id = Prev_Data.Customer_Id;
            if (PaymentDTO_Data.Customer_Loan_Id == 0) PaymentDTO_Data.Customer_Loan_Id = Prev_Data.Customer_Loan_Id;
            if (PaymentDTO_Data.Amount == 0.00f) PaymentDTO_Data.Amount = Prev_Data.Amount;
            if (PaymentDTO_Data.Payment_Date == DateTime.MinValue) PaymentDTO_Data.Payment_Date = Prev_Data.Payment_Date;
            return PaymentDTO_Data;
        }

        public static PaymentDTO Update(PaymentDTO PaymentDTO_Data)
        {
            PaymentDTO Prev_Data = Get(PaymentDTO_Data.Payment_Id);
            PaymentDTO_Data = Merge_Update(PaymentDTO_Data, Prev_Data);
            var Data = DataAccessFactory.PaymentData().Update(GetMapper().Map<Payment>(PaymentDTO_Data));
            return GetMapper().Map<PaymentDTO>(Data);
        }
        public static bool Delete(int id)
        {
            return DataAccessFactory.PaymentData().Delete(id);
        }
        public static List<PaymentDTO> Get_By_Customer(int customer_id)
        {
            List<PaymentDTO> PaymentDTO_Data = GetMapper().Map<List<PaymentDTO>>(DataAccessFactory.PaymentData().Get_By_Customer(customer_id));
            foreach (var item in PaymentDTO_Data)
            {
                item.CustomerDTO = CustomerService.Get(item.Customer_Id);
                item.CustomerLoanDTO = CustomerLoanService.Get(item.Customer_Loan_Id);
            }
            return PaymentDTO_Data;
        }
        public static List<PaymentDTO> Get_By_Customer_Loan(int Customer_Loan_Id, int Customer_Id)
        {
            List<PaymentDTO> PaymentDTO_Data = GetMapper().Map<List<PaymentDTO>>(DataAccessFactory.PaymentData().Get_By_Customer_Loan(Customer_Loan_Id, Customer_Id));
            foreach (var item in PaymentDTO_Data)
            {
                item.CustomerDTO = CustomerService.Get(item.Customer_Id);
                item.CustomerLoanDTO = CustomerLoanService.Get(item.Customer_Loan_Id);
            }
            return PaymentDTO_Data;
        }

    }
}
