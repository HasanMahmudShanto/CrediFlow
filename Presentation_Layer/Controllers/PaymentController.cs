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
    [RoutePrefix("api/payment")]
    public class PaymentController : ApiController
    {
        [HttpPost]
        [Route("LoanReturn")]
        public HttpResponseMessage LoanReturn(PaymentDTO PaymentDTO_Data)
        {
            CustomerLoanDTO Data = PaymentService.LoanReturn(PaymentDTO_Data);
            return Request.CreateResponse(HttpStatusCode.OK, Data);

        }
    }
}
