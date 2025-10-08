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
        public static List<LoanDTO> Get()
        {
            List<LoanDTO> LoanDTO_Data = GetMapper().Map<List<LoanDTO>>(DataAccessFactory.LoanData().Get());
            return LoanDTO_Data;
        }
        public static LoanDTO Get(int id)
        {
            LoanDTO LoanDTO_Data = GetMapper().Map<LoanDTO>(DataAccessFactory.LoanData().Get(id));
            return LoanDTO_Data;
        }
        public static float Calculate_Installment_Amount(int loanAmount, float interestRate, int durationMonths)
        {
            float monthlyInterestRate = interestRate / 12 / 100;
            int numberOfPayments = durationMonths;
            float installmentAmount = (loanAmount * monthlyInterestRate) / (1 - (float)Math.Pow(1 + monthlyInterestRate, -numberOfPayments));
            return installmentAmount;
        }
        public static LoanDTO Create(LoanDTO LoanDTO_Data)
        {
            if(LoanDTO_Data.Loan_Type != "Re-Activation Loan")
            {
                LoanDTO_Data.Installment_Amount = Calculate_Installment_Amount(LoanDTO_Data.Loan_Amount,
                    LoanDTO_Data.Interest_Percentage,
                    LoanDTO_Data.Loan_Duration_Months);
          
            }
            else
            {
                LoanDTO_Data.Installment_Amount = LoanDTO_Data.Loan_Amount;
            }
            var Data = DataAccessFactory.LoanData().Create(GetMapper().Map<Loan>(LoanDTO_Data));
            return GetMapper().Map<LoanDTO>(Data);
        }

        public static List<LoanDTO> Get_Eligible_Loans(int customer_id)
        {
            //Getting customer data
            CustomerDTO CustomerDTO_Data = CustomerService.Get(customer_id);

            //Null check
            if (CustomerDTO_Data == null) 
                return null;
            else
            {
                //Fetching eligible loans based on credit score
                List<LoanDTO> Eigible_Loans = GetMapper().Map<List<LoanDTO>>(DataAccessFactory.LoanData().Get_Eligible_Loans(CustomerDTO_Data.Credit_Score));
                return Eigible_Loans;
            }
        }
        public static LoanDTO Update(LoanDTO LoanDTO_Data)
        {
            var Data = DataAccessFactory.LoanData().Update(GetMapper().Map<Loan>(LoanDTO_Data));
            return GetMapper().Map<LoanDTO>(Data);
        }
        public static bool Delete(int id)
        {
            return DataAccessFactory.LoanData().Delete(id);
        }
    }
}
