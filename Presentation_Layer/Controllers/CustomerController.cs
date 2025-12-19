using BusinessLogic_Layer.DTOs;
using BusinessLogic_Layer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Presentation_Layer.Controllers
{
    [EnableCors(origins: "https://localhost:44388", headers: "*", methods: "*")]
    [RoutePrefix("api/customer")]
    public class CustomerController : ApiController
    {
        [HttpGet]
        [Route("all")]
        public HttpResponseMessage GetAllCustomers()
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

        [HttpGet]
        [Route("allpartial")]
        public HttpResponseMessage GetPartialCustomers()
        {
            try
            {
                var Data = CustomerService.GetPartial();
                return Request.CreateResponse(HttpStatusCode.OK, Data);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("create")]
        public HttpResponseMessage CreateCustomer(CustomerDTO CustomerDTO_Data)
        {
            
            if (!ModelState.IsValid)
            {
                // If DTO rules are violated, immediately return a 400 Bad Request 
                // with the error details.
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            // --- ONLY PROCEED IF VALIDATION PASSED ---
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
        public HttpResponseMessage GetLoansByCustomerId(int id)
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
        public HttpResponseMessage GetCustomerById(int id)
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
        public HttpResponseMessage UpdateCustomer(CustomerDTO CustomerDTO_Data)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            //Original Logic (Only runs if DTO is valid)
            try
            {
                CustomerDTO Data = CustomerService.Update(CustomerDTO_Data);

                if (Data == null)
                {
                    // This handles cases where the CustomerService determined the ID provided 
                    // in CustomerDTO_Data does not exist in the database.
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer not found.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Data);
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected service or database exceptions
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [Route("delete/{id}")]
        public HttpResponseMessage DeleteCustomer(int id)
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
