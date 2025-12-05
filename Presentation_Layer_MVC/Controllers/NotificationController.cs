using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation_Layer_MVC.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Upsert(int? id)
        {
            ViewBag.Notification_Id = id;
            return View();
        }
    }
}