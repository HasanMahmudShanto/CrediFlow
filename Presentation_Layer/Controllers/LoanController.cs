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
    public class LoanController : ApiController
    {

        [HttpGet]
        [Route("get_loan")]
        public HttpResponseMessage Get_Loan(int Customer_Id, int Loan_Id)
        {   
            var Data = LoanService.Get_Loan(Customer_Id, Loan_Id);
            return Request.CreateResponse(HttpStatusCode.OK, Data);
        }
    }
}
