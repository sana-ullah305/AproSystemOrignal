using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class BalanceSheetController : Appcode.BaseAprosysAccountingController
    {
        // GET: BalanceSheet
        public ActionResult Index()
        {
            return View();
        }
    }
}