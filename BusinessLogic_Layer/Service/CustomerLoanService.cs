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
            //Getting related data
            var Customer_Data = DataAccessFactory.CustomerData().Get(CustomerLoanDTO_Data.Customer_Id);
            var Loan_Data = DataAccessFactory.LoanData().Get(CustomerLoanDTO_Data.Loan_Id);

            //Null checks
            if (Customer_Data == null || Loan_Data == null) return null;
            if (Customer_Data.Credit_Score < Loan_Data.Minimum_Credit_Score) return null;

            //Checking if customer already has an active loan of the same type
            var Existing_Loans = DataAccessFactory.CustomerLoanData().Check_Existance_Of_Same_Loan(CustomerLoanDTO_Data.Customer_Id, CustomerLoanDTO_Data.Loan_Id);
            if (Existing_Loans) return null;

            //Populating remaining fields
            CustomerLoanDTO_Data.Loan_Taken_Date = DateTime.Now;

            //testing reminder feature for next installment date
            //CustomerLoanDTO_Data.Next_Installment_Date = DateTime.Now.AddDays(7);
            //CustomerLoanDTO_Data.Next_Installment_Date = DateTime.Now.AddDays(3);
            //CustomerLoanDTO_Data.Next_Installment_Date = DateTime.Now;

            CustomerLoanDTO_Data.Next_Installment_Date = DateTime.Now.AddMonths(1);
            CustomerLoanDTO_Data.Outstanding_Amount = Get_Outstanding_Amount(CustomerLoanDTO_Data.Loan_Id);
            CustomerLoanDTO_Data.CustomerDTO = GetMapper().Map<CustomerDTO>(Customer_Data);
            CustomerLoanDTO_Data.LoanDTO = GetMapper().Map<LoanDTO>(Loan_Data);
            CustomerLoanDTO_Data.Status = "Active";
            CustomerLoanDTO_Data.Next_Installment_Amount = CustomerLoanDTO_Data.LoanDTO.Installment_Amount;
            CustomerLoanDTO_Data.Total_Paid_Amount = 0.00f;
            CustomerLoanDTO_Data.Loan_End_Date = CustomerLoanDTO_Data.Loan_Taken_Date.AddMonths(CustomerLoanDTO_Data.LoanDTO.Loan_Duration_Months);
            
            //Creating CustomerLoan entry in DB
            CustomerLoanDTO Data = GetMapper().Map<CustomerLoanDTO>(DataAccessFactory.CustomerLoanData().Create(GetMapper().Map<CustomerLoan>(CustomerLoanDTO_Data)));
            Data.CustomerDTO = GetMapper().Map<CustomerDTO>(Customer_Data);
            Data.LoanDTO = GetMapper().Map<LoanDTO>(Loan_Data);


            //Creating Notification for the customer
            NotificationDTO Notification_Data = new NotificationDTO
            {
                Customer_Id = Data.Customer_Id,
                Title = "Loan Issued",
                Message = $"Dear {Customer_Data.Name},\n\n" +
                "We’re pleased to inform you that your loan has been successfully issued. Below are the details of your loan:\n\n" +
                $"• Loan Type: {Data.LoanDTO.Loan_Type}\n" +
                $"• Loan Amount: {Data.LoanDTO.Loan_Amount:C}\n" +
                $"• Installment Amount: {Data.LoanDTO.Installment_Amount:C}\n" +
                $"• Duration: {Data.LoanDTO.Loan_Duration_Months} months\n" +
                $"• Interest Rate: {Data.LoanDTO.Interest_Percentage}%\n" +
                $"• Next Installment Date: {Data.Next_Installment_Date:MMMM dd, yyyy}\n" +
                $"• Next Installment Amount: {Data.Next_Installment_Amount:C}\n" +
                $"• Total Payable Amount: {(Data.LoanDTO.Installment_Amount * Data.LoanDTO.Loan_Duration_Months):C}\n" +
                $"• Loan End Date: {Data.Loan_End_Date:MMMM dd, yyyy}\n\n" +
                "Thank you for choosing CrediFlow. We’re committed to supporting your financial journey.\n\n" +
                "Best regards,\nThe CrediFlow Team",
                CustomerDTO = GetMapper().Map<CustomerDTO>(Customer_Data)
            };
            bool Is_Created = NotificationService.Create(Notification_Data);

            //Returning data if notification is created successfully
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
        public static List <CustomerLoanDTO> Get()
        {
            List <CustomerLoanDTO> Data = GetMapper().Map<List<CustomerLoanDTO>>(DataAccessFactory.CustomerLoanData().Get());
            foreach(var item in Data)
            {
                item.LoanDTO = LoanService.Get(item.Loan_Id);
                item.CustomerDTO = CustomerService.Get(item.Customer_Id);
            }
                
            return Data;
        }

        public static List<CustomerLoanDTO> Get_All_Active_Loans()
        {
            List<CustomerLoanDTO> Data = GetMapper().Map<List<CustomerLoanDTO>>(DataAccessFactory.CustomerLoanData().Get_All_Active_Loans());
            return Data;
        }

        public static List<CustomerLoanDTO> Get_All_Closed_Loans()
        {
            List<CustomerLoanDTO> Data = GetMapper().Map<List<CustomerLoanDTO>>(DataAccessFactory.CustomerLoanData().Get_All_Closed_Loans());
            return Data;

        }

        public static CustomerLoanDTO Update(CustomerLoanDTO customerLoanDTO_Data)
        {
            var Data = DataAccessFactory.CustomerLoanData().Update(GetMapper().Map<CustomerLoan>(customerLoanDTO_Data));
            return GetMapper().Map<CustomerLoanDTO>(Data);

        }

        public static bool Delete(int id)
        {
            return DataAccessFactory.CustomerLoanData().Delete(id);
        }
    }
}
