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
    public class LoanController : ApiController
    {
        [HttpGet]
        [Route("all")]
        public HttpResponseMessage Get()
        {
            try
            {
                List<LoanDTO> Data = LoanService.Get();
                if(Data == null || Data.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No loans found.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Data);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            try
            {

                LoanDTO Data = LoanService.Get(id);
                if(Data == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Loan not found.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Data);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }
        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(LoanDTO LoanDTO_Data) 
        {
            try
            {

                LoanDTO Data = LoanService.Create(LoanDTO_Data);
                if(Data == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to create loan.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Data);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }
        [HttpGet]
        [Route("Get_eligible_loans/{customer_id}")]
        public HttpResponseMessage Get_eligible_loans(int customer_id)
        {
            try
            {
                if (CustomerService.Get(customer_id) == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Customer not found.");
                }
                else
                {
                    List<LoanDTO> Eligible_Loans = LoanService.Get_Eligible_Loans(customer_id);
                    if (Eligible_Loans == null || Eligible_Loans.Count == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "No eligible loans found for the customer.");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, Eligible_Loans);
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }
        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(LoanDTO LoanDTO_Data)
        {
            try
            {

                LoanDTO Data = LoanService.Update(LoanDTO_Data);
                if(Data == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to update loan.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Data);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }
        [HttpPost]
        [Route("delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                bool isDeleted = LoanService.Delete(id);
                if(!isDeleted)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to delete loan.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Loan deleted successfully.");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }
    }
}
