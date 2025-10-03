using DataAccess_Layer.Interfaces;
using DataAccess_Layer.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer
{
    public class DataAccessFactory
    {

        public static ICustomerRepo CustomerData()
        {
            return new CustomerRepo();
        }
        public static ILoanRepo LoanData()
        {
            return new LoanRepo();
        }
        public static ICustomerLoanRepo CustomerLoanData() 
        {
            return new CustomerLoanRepo();
        }
        public static INotificationRepo NotificationData()
        {
            return new NotificationRepo();
        }

        public static IPaymentRepo PaymentData()
        {
            return new PaymentRepo();
        }
    }
}
