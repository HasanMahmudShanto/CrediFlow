using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Presentation_Layer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // Call the method to check and send reminders when the application starts
            BusinessLogic_Layer.Service.ReminderService.Get_Reminders();
            BusinessLogic_Layer.Service.LateFeeService.Apply_Late_Fee();
        }
    }
}


