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
            var Data = CustomerService.Get();
            return Request.CreateResponse(HttpStatusCode.OK, Data);
        }
        [HttpPost]
        [Route("register")]
        public HttpResponseMessage Register(CustomerDTO CustomerDTO_Data)
        {
            CustomerDTO Data = CustomerService.Register(CustomerDTO_Data);
            return Request.CreateResponse(HttpStatusCode.OK, Data);
        }
    }
}
