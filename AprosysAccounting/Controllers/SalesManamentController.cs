using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class SalesManamentController : Appcode.BaseAprosysAccountingController
    {
        // GET: SalesManament
        public ActionResult Index()
        {
            return View();
        }
    }
}