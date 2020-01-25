using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class SalesReturnController : Appcode.BaseAprosysAccountingController
    {
        // GET: SalesReturn
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetOrderDetailsByInvoiceNo(string invoiceNo)
        {
            return Json(BL_SalesReturn.GetOrderDetailsByInvoiceNo(invoiceNo), JsonRequestBehavior.AllowGet);
        }
        public string GetSaleInvoicesList()
        {
            return BL_Common.Serialize(BL_SalesReturn.GetSaleInvoicesList());
        }

        public JsonResult SaveSalesReturn(BO_SalesReturn salesReturn)
        {
            return Json(BL_SalesReturn.SaveSalesReturn(salesReturn, UserAprosysAccounting.id), JsonRequestBehavior.AllowGet);
        }
    }
}