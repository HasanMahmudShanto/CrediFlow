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
                LoanDTO_Data.Loan_Duration_Months = 1; //Re-Activation loan duration is 1 month
                LoanDTO_Data.Interest_Percentage = 0; //Re-Activation loan has no interest
                LoanDTO_Data.Penalty_Percentage = 0; //Re-Activation loan has no penalty
                LoanDTO_Data.Minimum_Credit_Score = 200.0f;
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

        public static LoanDTO Merge_Update(LoanDTO loanDTO, LoanDTO Prev_Data)
        {
            if(loanDTO.Loan_Type == null) loanDTO.Loan_Type = Prev_Data.Loan_Type;
            if(loanDTO.Loan_Duration_Months == 0) loanDTO.Loan_Duration_Months = Prev_Data.Loan_Duration_Months;
            if(loanDTO.Interest_Percentage == 0) loanDTO.Interest_Percentage = Prev_Data.Interest_Percentage;
            if(loanDTO.Penalty_Percentage == 0) loanDTO.Penalty_Percentage = Prev_Data.Penalty_Percentage;
            if(loanDTO.Loan_Amount == 0) loanDTO.Loan_Amount = Prev_Data.Loan_Amount;
            if(loanDTO.Minimum_Credit_Score == 0) loanDTO.Minimum_Credit_Score = Prev_Data.Minimum_Credit_Score;
            if(loanDTO.Loan_Type != "Re-Activation Loan")
            {
                loanDTO.Installment_Amount = Calculate_Installment_Amount(loanDTO.Loan_Amount,
                    loanDTO.Interest_Percentage,
                    loanDTO.Loan_Duration_Months);
            }
            else
            {
                loanDTO.Installment_Amount = loanDTO.Loan_Amount;
            }
            return loanDTO;
        }

        public static LoanDTO Update(LoanDTO LoanDTO_Data)
        {
            LoanDTO Prev_Data = Get(LoanDTO_Data.Loan_Id);
            LoanDTO_Data = Merge_Update(LoanDTO_Data, Prev_Data);
            var Data = DataAccessFactory.LoanData().Update(GetMapper().Map<Loan>(LoanDTO_Data));
            return GetMapper().Map<LoanDTO>(Data);
        }
        public static bool Delete(int id)
        {
            return DataAccessFactory.LoanData().Delete(id);
        }
    }
}
