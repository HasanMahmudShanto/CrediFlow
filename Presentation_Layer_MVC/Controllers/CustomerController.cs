using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation_Layer_MVC.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Upsert(int? id)
        {
            ViewBag.Customer_Id = id;
            ViewBag.Title = id == null ? "Create Customer" : "Edit Customer";
            return View();
        }
    }
}