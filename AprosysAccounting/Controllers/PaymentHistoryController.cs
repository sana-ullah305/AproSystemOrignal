using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class PaymentHistoryController : Appcode.BaseAprosysAccountingController
    {
        // GET: PaymentHistory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPaymentHistoryList(JQueryDataTableParamModel Param)
        {

            MYJSONTblCustom MYJSON = BL_PaymentHistory.GetPaymentHistoryList(Param, Request, UserAprosysAccounting.id);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }

        public string DeletePaymentHistory(int tranId)
        {
            BL_PaymentHistory obj = new BussinessLogics.BL_PaymentHistory();

            return BL_Common.Serialize(obj.DeletePaymentHistory(tranId, UserAprosysAccounting.id));
        }
    }
}