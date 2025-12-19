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
    [RoutePrefix("api/customerloan")]

    public class CustomerLoanController : ApiController
    {

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage CreateCustomerLoan(CustomerLoanDTO CustomerLoanDTO_Data)
        {
            CustomerLoanDTO Data = CustomerLoanService.Get_Loan(CustomerLoanDTO_Data);
            return Request.CreateResponse(HttpStatusCode.OK, Data);
        }
        [HttpGet]
        [Route("get/{id}")]
        public HttpResponseMessage GetCustomerLoanById(int id)
        {
            CustomerLoanDTO Data = CustomerLoanService.Get(id);
            return Request.CreateResponse(HttpStatusCode.OK, Data);
        }
        [HttpGet]
        [Route("all")]
        public HttpResponseMessage GetAllCustomerLoans()
        {
            try
            {

                var Data = CustomerLoanService.Get();
                if (Data == null || Data.Count == 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No loans found.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Data);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("active_loans")]
        public HttpResponseMessage GetActiveLoans()
        {
            try
            {
                List<CustomerLoanDTO> Data = CustomerLoanService.Get_All_Active_Loans();
                if (Data == null || Data.Count == 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No active loans found for the customer.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Data);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("closed_loans")]
        public HttpResponseMessage GetClosedLoans()
        {
            try
            {
                List<CustomerLoanDTO> Data = CustomerLoanService.Get_All_Closed_Loans();
                if (Data == null || Data.Count == 0)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No closed loans found for the customer.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Data);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [Route("update")]
        public HttpResponseMessage UpdateCustomerLoans(CustomerLoanDTO CustomerLoanDTO_Data)
        {
            try
            {
                CustomerLoanDTO Data = CustomerLoanService.Update(CustomerLoanDTO_Data);
                if (Data == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Loan not found or update failed.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Data);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [Route("delete/{id}")]
        public HttpResponseMessage DeleteCustomerLoans(int id)
        {
            try
            {
                bool Is_Deleted = CustomerLoanService.Delete(id);
                if (Is_Deleted)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Loan deleted successfully.");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Loan not found or deletion failed.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
