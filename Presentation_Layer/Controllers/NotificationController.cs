using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Presentation_Layer.Controllers
{
    [RoutePrefix("api/notification")]
    public class NotificationController : ApiController
    {
        [HttpGet]
        [Route("all")]
        public HttpResponseMessage Get()
        {
            var Data = BusinessLogic_Layer.Service.NotificationService.Get();
            return Request.CreateResponse(HttpStatusCode.OK, Data);
        }
    }
}
