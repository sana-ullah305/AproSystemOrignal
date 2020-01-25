using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessLogics;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class RoutineController : Controller
    {
        // GET: Routine
        public ActionResult Index()
        {
            return View();
        }
        public string ValidateDueDates(string actDate)
        {
            DateTime date = String.IsNullOrEmpty(actDate) ? DateTime.Now.Date : Convert.ToDateTime(actDate);
            return BL_SubscriptionManagement.ValidateDueDates(date);
        }
    }
}