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
    public class CustomerLoanService
    {

        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<CustomerLoan, CustomerLoanDTO>().ReverseMap();
                cfg.CreateMap<Customer, CustomerDTO>().ReverseMap();
                cfg.CreateMap<Loan, LoanDTO>().ReverseMap();
            });
            return new Mapper(config);
        }
        public static float Get_Outstanding_Amount(int id)
        {
            var Data = DataAccessFactory.LoanData().Get(id);
            return Data.Installment_Amount * Data.Loan_Duration_Months;
        }

        public static CustomerLoanDTO Get_Loan(CustomerLoanDTO CustomerLoanDTO_Data)
        {
            var Customer_Data = DataAccessFactory.CustomerData().Get(CustomerLoanDTO_Data.Customer_Id);
            var Loan_Data = DataAccessFactory.LoanData().Get(CustomerLoanDTO_Data.Loan_Id);

            CustomerLoanDTO_Data.Loan_Taken_Date = DateTime.Now;
            CustomerLoanDTO_Data.Next_Installment_Date = DateTime.Now.AddMonths(1);
            CustomerLoanDTO_Data.Outstanding_Amount = Get_Outstanding_Amount(CustomerLoanDTO_Data.Loan_Id);
            CustomerLoanDTO_Data.CustomerDTO = GetMapper().Map<CustomerDTO>(Customer_Data);
            CustomerLoanDTO_Data.LoanDTO = GetMapper().Map<LoanDTO>(Loan_Data);

            CustomerLoanDTO_Data.Next_Installment_Amount = CustomerLoanDTO_Data.LoanDTO.Installment_Amount;
            CustomerLoanDTO_Data.Total_Paid_Amount = 0.00f;
            CustomerLoanDTO Data = GetMapper().Map<CustomerLoanDTO>(DataAccessFactory.CustomerLoanData().Create(GetMapper().Map<CustomerLoan>(CustomerLoanDTO_Data)));
            Data.CustomerDTO = GetMapper().Map<CustomerDTO>(Customer_Data);
            Data.LoanDTO = GetMapper().Map<LoanDTO>(Loan_Data);

            NotificationDTO Notification_Data = new NotificationDTO
            {
                CustomerId = Data.Customer_Id,
                Date = DateTime.Now,
                IsRead = false,
                Title = "Loan Issued",
                Message = "Your loan has been successfully issued"
            };
            bool Is_Created = NotificationService.Create(Notification_Data);
            if (Is_Created)
                return Data;
            return null;
        }

        public static CustomerLoanDTO Get(int Customer_Loan_Id)
        {
            CustomerLoanDTO Data = GetMapper().Map<CustomerLoanDTO>(DataAccessFactory.CustomerLoanData().Get(Customer_Loan_Id));
            Data.LoanDTO = LoanService.Get(Data.Loan_Id);
            Data.CustomerDTO = CustomerService.Get(Data.Customer_Id);
            return Data;
        }
    }
}
