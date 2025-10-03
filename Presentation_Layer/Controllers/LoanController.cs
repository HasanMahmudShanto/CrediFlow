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
            List<LoanDTO> Data = LoanService.Get();
            return Request.CreateResponse(HttpStatusCode.OK, Data);
        }
        [HttpGet]
        [Route("id/{id}")]
        public HttpResponseMessage Get(int id)
        {
            LoanDTO Data = LoanService.Get(id);
            return Request.CreateResponse(HttpStatusCode.OK, Data);
        }
        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(LoanDTO LoanDTO_Data) 
        {
            LoanDTO Data = LoanService.Create(LoanDTO_Data);
            return Request.CreateResponse(HttpStatusCode.OK, Data);
        }
    }
}
