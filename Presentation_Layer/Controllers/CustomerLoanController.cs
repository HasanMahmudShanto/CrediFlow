using BusinessLogic_Layer.DTOs;
using BusinessLogic_Layer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Presentation_Layer.Controllers
{
    [RoutePrefix("api/loan")]

    public class CustomerLoanController : ApiController
    {

        [HttpPost]
        [Route("get_loan")]
        public HttpResponseMessage Get_Loan(CustomerLoanDTO CustomerLoanDTO_Data)
        {
            CustomerLoanDTO Data = CustomerLoanService.Get_Loan(CustomerLoanDTO_Data);

            NotificationDTO Notification_Data = new NotificationDTO
            {
                CustomerId = Data.Customer_Id,
                Date = DateTime.Now,
                IsRead = false,
                Title = "Loan Issued",
                Message = "Your loan has been successfully issued"
          
            };
            NotificationService.Create(Notification_Data);
            return Request.CreateResponse(HttpStatusCode.OK, Data);
        }

    }
}
