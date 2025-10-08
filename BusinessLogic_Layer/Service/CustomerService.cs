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
    public class CustomerService
    {

        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Customer, CustomerDTO>().ReverseMap();
                cfg.CreateMap<CustomerLoan, CustomerLoanDTO>().ReverseMap();
                cfg.CreateMap<Loan, LoanDTO>().ReverseMap();
                cfg.CreateMap<Notification, NotificationDTO>().ReverseMap();
                cfg.CreateMap<Payment, PaymentDTO>().ReverseMap();
            });
            return new Mapper(config);
        }

        public static float Calculate_Credit_Score(float Monthly_Income)
        {
            if (Monthly_Income < 15000) return 350;
            if (Monthly_Income < 30000) return 450;
            if (Monthly_Income < 50000) return 500;
            if (Monthly_Income < 80000) return 600;
            if (Monthly_Income < 120000) return 650;
            return 700;
        }
        public static string Determine_Status(float Credit_Score)
        {
            if (Credit_Score < 200) return "Terminated";
            if (Credit_Score < 300) return "Restricted";
            if (Credit_Score < 500) return "Poor";
            if (Credit_Score < 650) return "Fair";
            if (Credit_Score < 750) return "Good";
            return "Excellent";
        }
        public static List<CustomerDTO> Get()
        {
            var Data = DataAccessFactory.CustomerData().Get();
            return GetMapper().Map<List<CustomerDTO>>(Data);
        }
        
        public static CustomerDTO Get(int id)
        {
            var Data = DataAccessFactory.CustomerData().Get(id);
            return GetMapper().Map<CustomerDTO>(Data);
        }



        public static CustomerDTO Create(CustomerDTO CustomerDTO_Data)
        {
            //Populating remaining fields
            CustomerDTO_Data.Credit_Score = Calculate_Credit_Score(CustomerDTO_Data.Monthly_Income);
            CustomerDTO_Data.Status = Determine_Status(CustomerDTO_Data.Credit_Score);
            var Data = DataAccessFactory.CustomerData().Create(GetMapper().Map<Customer>(CustomerDTO_Data));

            //Creating Notification for the customer
            var NotificationDTO_Data = new NotificationDTO
            {
                Customer_Id = Data.Customer_Id,
                Title = "Account Created",
                Message = $"Dear {CustomerDTO_Data.Name},\n\n" +
                "Welcome to CrediFlow! Your account has been successfully created.\n\n" +
                "Here are your account details:\n" +
                $"• Name: {Data.Name}\n" +
                $"• Email: {Data.Email}\n" +
                $"• Monthly Income: {Data.Monthly_Income}\n" +
                $"• Credit Score: {Data.Credit_Score}\n" +
                $"• Status: {Data.Status}\n\n" +
                "We’re excited to have you with us. You can now log in and start exploring our services.\n\n" +
                "Best regards,\nThe CrediFlow Team",

                CustomerDTO = GetMapper().Map<CustomerDTO>(Data)
            };
            bool Notification_Data = NotificationService.Create(NotificationDTO_Data);

            if (!Notification_Data) return null;
            return GetMapper().Map<CustomerDTO>(Data);
        }

        public static List<CustomerLoanDTO> Get_Loans(int id)
        {
            List<CustomerLoanDTO> Data =GetMapper().Map<List<CustomerLoanDTO>>(DataAccessFactory.CustomerLoanData().Get_By_Customer(id));
            foreach (var item in Data)
            {
                item.LoanDTO = LoanService.Get(item.Loan_Id);
                item.CustomerDTO = Get(id);
            };
            return Data;
        }
        public static CustomerDTO Update(CustomerDTO CustomerDTO_Data)
        {
            CustomerDTO_Data.Status = Determine_Status(CustomerDTO_Data.Credit_Score);
            var Data = DataAccessFactory.CustomerData().Update(GetMapper().Map<Customer>(CustomerDTO_Data));

           
            return GetMapper().Map<CustomerDTO>(Data);
        }
        public static bool Delete(int id)
        {
            var NotificationDTO_Data = new NotificationDTO
            {
                Customer_Id = id,
                Title = "Profile Deleted",
                Message = "Dear Customer,\n\n" +
                "Your CrediFlow profile has been successfully deleted from our system.\n" +
                "If you did not request this action, please contact our support team immediately to secure your account.\n\n" +
                "Thank you for being a part of CrediFlow.\n\n" +
                "Best regards,\nThe CrediFlow Team"
            };
            bool Is_Created = NotificationService.Create(NotificationDTO_Data);
            if (!Is_Created) return false;
            return DataAccessFactory.CustomerData().Delete(id);
        }
    }
}
