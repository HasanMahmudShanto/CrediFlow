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
    [RoutePrefix("api/customer")]
    public class CustomerController : ApiController
    {
        [HttpGet]
        [Route("all")]
        public HttpResponseMessage Get()
        {
            try
            {
                var Data = CustomerService.Get();
                return Request.CreateResponse(HttpStatusCode.OK, Data);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(CustomerDTO CustomerDTO_Data)
        {
            try
            {

                CustomerDTO Data = CustomerService.Create(CustomerDTO_Data);
                return Request.CreateResponse(HttpStatusCode.OK, Data);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("customer_loans/{id}")]
        public HttpResponseMessage Get_Loans(int id)
        {
            try
            {
                List<CustomerLoanDTO> Data = CustomerService.Get_Loans(id);
                if (Data == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer not found.");
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
        [Route("customer/{id}")]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var Data = CustomerService.Get(id);
                if (Data == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer not found.");
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
        public HttpResponseMessage Update(CustomerDTO CustomerDTO_Data)
        {
            try
            {

                CustomerDTO Data = CustomerService.Update(CustomerDTO_Data);
                if (Data == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer not found.");
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
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var customer = CustomerService.Get(id);
                if (customer == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer not found.");
                }
                else
                {
                    bool isDeleted = CustomerService.Delete(id);
                    if (isDeleted)
                        return Request.CreateResponse(HttpStatusCode.OK, "Customer deleted successfully.");
                    else
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to delete customer.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
