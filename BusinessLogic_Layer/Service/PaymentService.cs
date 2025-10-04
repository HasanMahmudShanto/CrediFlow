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
            if (CustomerLoanDTO_Data == null) 
                return null;
            else if(CustomerLoanDTO_Data.Outstanding_Amount < PaymentDTO_Data.Amount || PaymentDTO_Data.Amount <= 0
                || CustomerLoanDTO_Data.Outstanding_Amount == 0) 
                return null;

            //Processing payment
            PaymentDTO_Data.Payment_Date = DateTime.Now;
            PaymentDTO_Data.CustomerDTO = CustomerDTO_Data;
            PaymentDTO_Data.CustomerLoanDTO = CustomerLoanDTO_Data; // Including late fee if any
            PaymentDTO_Data.Amount = CustomerLoanDTO_Data.Next_Installment_Amount;


            //CustomerLoanDTO_Data.Next_Installment_Date = DateTime.Now.AddDays(-1); // Forcing late fee for testing
            // Late fee calculation
            float Late_Fee = 0.00f;
            if(CustomerLoanDTO_Data.Next_Installment_Date - DateTime.Now >= TimeSpan.Zero)
            {
                Late_Fee = 0.00f;
            }
            else
            {
                Late_Fee = 0.1f * PaymentDTO_Data.CustomerLoanDTO.LoanDTO.Installment_Amount; // Fixed late fee for simplicity
            }

            //Updating CustomerLoanDTO_Data
            CustomerLoanDTO_Data.Next_Installment_Date = CustomerLoanDTO_Data.Next_Installment_Date.AddMonths(1);
            CustomerLoanDTO_Data.Next_Installment_Amount = CustomerLoanDTO_Data.LoanDTO.Installment_Amount + Late_Fee;
            CustomerLoanDTO_Data.Outstanding_Amount += Late_Fee;
            CustomerLoanDTO_Data.Total_Paid_Amount += PaymentDTO_Data.Amount;

            //Saving payment in DB
            var Payment_Data = DataAccessFactory.PaymentData().Create(GetMapper().Map<Payment>(PaymentDTO_Data));

            //Updating CustomerLoan in DB and returning updated data
            if (Payment_Data)
            {
                CustomerLoanDTO_Data.Outstanding_Amount -= PaymentDTO_Data.Amount;
                
                CustomerLoanDTO Data = GetMapper().Map<CustomerLoanDTO>(DataAccessFactory.CustomerLoanData().Update(GetMapper().Map<CustomerLoan>(CustomerLoanDTO_Data)));
                Data.CustomerDTO = CustomerDTO_Data;
                Data.LoanDTO = LoanDTO_Data;
                return Data;
            }
            return null;
        }
    }
}
