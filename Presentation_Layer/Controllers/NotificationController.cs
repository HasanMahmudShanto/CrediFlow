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
    [RoutePrefix("api/notification")]
    public class NotificationController : ApiController
    {
        [HttpGet]
        [Route("all")]
        public HttpResponseMessage Get()
        {
            try
            {

                var Data = BusinessLogic_Layer.Service.NotificationService.Get();
                if(Data == null || Data.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No notifications found.");
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
        [Route("allpartial")]
        public HttpResponseMessage Get_Partial()
        {
            try
            {
                var Data = BusinessLogic_Layer.Service.NotificationService.GetPartial();
                if(Data == null || Data.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No notifications found.");
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
                var Data = BusinessLogic_Layer.Service.NotificationService.Get(id);
                if(Data == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Notification not found.");
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
        [Route("customer/{id}")]
        public HttpResponseMessage Get_By_Customer(int id)
        {
            try
            {

                var Data = BusinessLogic_Layer.Service.NotificationService.Get_By_Customer(id);
                if(Data == null || Data.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No notifications found for the customer.");
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
        public HttpResponseMessage Create(NotificationDTO NotificationDTO_Data)
        {
            
            if (!ModelState.IsValid)
            {
                
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            // Original Logic (Only runs if DTO is valid)
            try
            {
                bool is_Create = NotificationService.Create(NotificationDTO_Data);
                if (is_Create)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Notification created successfully.");
                }
                else
                {
                    // Internal error in the service/data access layer
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to create notification.");
                }
            }
            catch (Exception ex)
            {
                // Catch unexpected exceptions
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }
        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(NotificationDTO NotificationDTO_Data)
        {
            
            if (!ModelState.IsValid)
            {
                
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            // Original Logic (Only runs if DTO is valid)
            try
            {
                bool is_Update = NotificationService.Update(NotificationDTO_Data);
                if (is_Update)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Notification updated successfully.");
                }
                else
                {
                    // Internal error or update failed (e.g., ID not found)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to update notification.");
                }
            }
            catch (Exception ex)
            {
                // Catch unexpected exceptions
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                bool is_Delete = BusinessLogic_Layer.Service.NotificationService.Delete(id);
                if (is_Delete)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Notification deleted successfully.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to delete notification.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "An error occurred: " + ex.Message);
            }
        }
    }
}
