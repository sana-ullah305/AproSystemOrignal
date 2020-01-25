using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class ReportBankStatementController : Appcode.BaseAprosysAccountingController
    {
        // GET: ReportBankStatement
        public ActionResult Index()
        {
            return View();
        }
    }
}