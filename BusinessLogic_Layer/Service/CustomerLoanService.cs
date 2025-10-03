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
            });
            return new Mapper(config);
        }
        public static CustomerLoanDTO Get_Loan(CustomerLoanDTO CustomerLoanDTO_Data)
        {

            CustomerLoanDTO_Data.Loan_Taken_Date = DateTime.Now;
            CustomerLoanDTO_Data.Outstanding_Amount = DataAccessFactory.LoanData().Get(CustomerLoanDTO_Data.Loan_Id).Loan_Amount;

            var Data = DataAccessFactory.CustomerLoanData().Create(GetMapper().Map<CustomerLoan>(CustomerLoanDTO_Data));
            return GetMapper().Map<CustomerLoanDTO>(Data);
        }

        public static CustomerLoanDTO Get(int Customer_Loan_Id)
        {
            var Data = DataAccessFactory.CustomerLoanData().Get(Customer_Loan_Id);
            return GetMapper().Map<CustomerLoanDTO>(Data);
        }
    }
}
