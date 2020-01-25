using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class CashFlowReportController : Appcode.BaseAprosysAccountingController
    {
        // GET: CashFlowReport
        public ActionResult Index()
        {
            return View();
        }
    }
}