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
            try
            {

                CustomerLoanDTO Data = PaymentService.LoanReturn(PaymentDTO_Data);
                if(Data == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Payment could not be processed.");
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
        [Route("all")]
        public HttpResponseMessage GetAllPayments()
        {
            try
            {
                var Data = BusinessLogic_Layer.Service.PaymentService.Get();
                if(Data == null || Data.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No payments found.");
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
        public HttpResponseMessage GetPaymentById(int id)
        {
            try
            {
                var Data = BusinessLogic_Layer.Service.PaymentService.Get(id);
                if(Data == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Payment not found.");
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
        public HttpResponseMessage CreatePayment(PaymentDTO PaymentDTO_Data)
        {
            try
            {
                PaymentDTO Data = BusinessLogic_Layer.Service.PaymentService.Create(PaymentDTO_Data);
                if(Data == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to create payment.");
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
        [Route("update")]
        public HttpResponseMessage UpdatePayment(PaymentDTO PaymentDTO_Data)
        {
            try
            {
                PaymentDTO Data = BusinessLogic_Layer.Service.PaymentService.Update(PaymentDTO_Data);
                if(Data == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to update payment.");
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
        public HttpResponseMessage DeletePayment(int id)
        {
            try
            {
                bool is_Delete = BusinessLogic_Layer.Service.PaymentService.Delete(id);
                if(is_Delete)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Payment deleted successfully.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to delete payment.");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }
    }
}
