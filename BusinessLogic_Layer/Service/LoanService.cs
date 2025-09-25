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
    public class LoanService
    {

        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Customer, CustomerDTO>().ReverseMap();
                cfg.CreateMap<CustomerLoan, CustomerLoanDTO>().ReverseMap();
                cfg.CreateMap<Loan, LoanDTO>().ReverseMap();
            });
            return new Mapper(config);
        }

        public static List<CustomerDTO> Get()
        {
            var Data = DataAccessFactory.CustomerData().Get();
            return GetMapper().Map<List<CustomerDTO>>(Data);
        }

        public static CustomerLoanDTO Get_Loan(int Customer_Id, int Loan_Id)
        {
            var Data = DataAccessFactory.CustomerLoanData().Get(id);
            return GetMapper().Map<CustomerLoanDTO>(Data);
        }
        public static object Get_Loan(int Customer_Id, int Loan_Id)
        {
            // Business logic to retrieve loan details based on Customer_Id and Loan_Id
            // This is a placeholder implementation
            return new { CustomerId = Customer_Id, LoanId = Loan_Id, Amount = 10000, Status = "Approved" };
        }
    }
}
