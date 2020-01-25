using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class SubscriptionReportController : Appcode.BaseAprosysAccountingController
    {
        // GET: SubscriptionReport
        public ActionResult Index()
        {
            return View();
        }
    }
}