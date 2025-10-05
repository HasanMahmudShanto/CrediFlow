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

        public static CustomerLoanDTO LoanReturn(PaymentDTO PaymentDTO_Data)
        {
            //Getting related data
            CustomerDTO CustomerDTO_Data = CustomerService.Get(PaymentDTO_Data.Customer_Id);
            CustomerLoanDTO CustomerLoanDTO_Data = CustomerLoanService.Get(PaymentDTO_Data.Customer_Loan_Id);
            LoanDTO LoanDTO_Data = LoanService.Get(CustomerLoanDTO_Data.Loan_Id);

            //Null checks and validations

            //Checking if related data exists
            if (CustomerLoanDTO_Data == null || CustomerDTO_Data == null || LoanDTO_Data == null) 
                return null;
            //Checking if the payment amount is valid
            else if (CustomerLoanDTO_Data.Outstanding_Amount < PaymentDTO_Data.Amount || CustomerLoanDTO_Data.Outstanding_Amount == 0) 
                return null;
            //Checking if the loan is already closed or customer is terminated
            if (CustomerDTO_Data.Status == "Terminated" || CustomerLoanDTO_Data.Status == "Closed") 
                return null;
            //Ensuring the payment is being made by the rightful customer
            if (PaymentDTO_Data.Customer_Id != CustomerLoanDTO_Data.Customer_Id) 
                return null;


            //Processing payment
            PaymentDTO_Data.Payment_Date = DateTime.Now;
            PaymentDTO_Data.CustomerDTO = CustomerDTO_Data;
            PaymentDTO_Data.CustomerLoanDTO = CustomerLoanDTO_Data; 
            PaymentDTO_Data.Amount = CustomerLoanDTO_Data.Next_Installment_Amount;

            //force late payment for testing
           // CustomerLoanDTO_Data.Next_Installment_Date = CustomerLoanDTO_Data.Next_Installment_Date.AddMonths(-12);

            // Late fee calculation
            float Late_Fee = 0.00f;
            if(CustomerLoanDTO_Data.Next_Installment_Date - DateTime.Now >= TimeSpan.Zero) // On-time payment
            {
                if(CustomerDTO_Data.Status == "Restricted") //Re-activating restricted customers
                {
                    CustomerDTO_Data.Status = "Active";
                    Late_Fee = 0.00f;
                    CustomerDTO_Data.Credit_Score = 300; // Reset credit score upon re-activation
                    CustomerDTO_Data.Status = "Poor";
                }
                else // Normal Monthly Installment Payment
                {
                    Late_Fee = 0.00f;
                    CustomerDTO_Data.Credit_Score += 20; // Increase credit score for on-time payment
                }
            }
            else // Late payment
            {
                if(CustomerDTO_Data.Status == "Restricted") // Restricted customers making late payments (Terminate them)
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
                    Data.LoanDTO = LoanDTO_Data;

                    //Saving updated customer data in DB
                    CustomerDTO Updated_Customer_Data = CustomerService.Update(CustomerDTO_Data);

                    //Returning null as the customer is terminated
                    return null;
                }
                else // Normal Monthly Installment Payment
                {
                    Late_Fee = 0.1f * PaymentDTO_Data.CustomerLoanDTO.LoanDTO.Installment_Amount; // 10% late fee on monthly installment
                    CustomerDTO_Data.Credit_Score -= 25; // Decrease credit score for late payment
                }
            }

            //Ensuring credit score remains within bounds
            if (CustomerDTO_Data.Credit_Score > 900) CustomerDTO_Data.Credit_Score = 900;
            else if (CustomerDTO_Data.Credit_Score < 0) CustomerDTO_Data.Credit_Score = 0;


            //Updating CustomerLoanDTO_Data

            CustomerLoanDTO_Data.Outstanding_Amount -= PaymentDTO_Data.Amount;

            //If there are some decimals left due to float calculations,
            //we consider the loan fully paid if outstanding amount is less than 1 currency unit
            if (CustomerLoanDTO_Data.Outstanding_Amount < 1.00f && CustomerLoanDTO_Data.Outstanding_Amount > 0.00f) 
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
                CustomerLoanDTO_Data.Next_Installment_Amount = CustomerLoanDTO_Data.LoanDTO.Installment_Amount + Late_Fee;
                CustomerLoanDTO_Data.Outstanding_Amount += Late_Fee;
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
                return Data;
            }
            return null;
        }
    }
}
