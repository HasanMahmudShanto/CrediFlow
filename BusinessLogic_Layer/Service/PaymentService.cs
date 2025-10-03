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
            });
            return new Mapper(config);
        }

        public static CustomerLoanDTO LoanReturn(PaymentDTO PaymentDTO_Data)
        {
            PaymentDTO_Data.Payment_Date = DateTime.Now;
            var Payment_Data = DataAccessFactory.PaymentData().Create(GetMapper().Map<Payment>(PaymentDTO_Data));
            if (Payment_Data)
            {
                CustomerLoanDTO CustomerLoanDTO_Data = CustomerLoanService.Get(PaymentDTO_Data.Customer_Loan_Id);
                CustomerLoanDTO_Data.Outstanding_Amount -= PaymentDTO_Data.Amount;
                
                var Data = DataAccessFactory.CustomerLoanData().Update(GetMapper().Map<CustomerLoan>(CustomerLoanDTO_Data));
                return GetMapper().Map<CustomerLoanDTO>(Data);
            }
            return null;
        }
    }
}
