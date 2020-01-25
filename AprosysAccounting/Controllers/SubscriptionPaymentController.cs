using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;

namespace AprosysAccounting.Controllers
{
    public class SubscriptionPaymentController : Appcode.BaseAprosysAccountingController
    {
        // GET: SubscriptionPayment
        public ActionResult Index()
        {
            return View();
        }
        //public string GetNextVoucher()
        //{
        //    string voucher = BL_Common.GetLastVoucher(3);
        //    if (String.IsNullOrEmpty(voucher)) { return BL_Common.Serialize("RCT-1"); }
        //    var increment = Convert.ToInt32(voucher.Split('-')[1]) + 1;
        //    return BL_Common.Serialize("RCT-" + increment);
        //}
        public ActionResult LoadReceiptVoucherTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_SubscriptionPayment.LoadSubscriptionPaymentVoucherTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }
        public string SaveReceiptVoucher(string rvoucher)
        {
            var _rvoucher = BL_Common.Deserialize<BO_ReceiptVoucher>(rvoucher);
            return BL_Common.Serialize((BL_ReceiptVoucher.SaveReceiptVoucher(_rvoucher)));

        }

        public string DeleteReceiptVoucher(string invoiceNo)
        {
            var pl = BL_ReceiptVoucher.DeleteReceiptVoucher(invoiceNo, UserAprosysAccounting.id);
            return BL_Common.Serialize("success");
        }
        public ActionResult GetReceiptVoucherByInvoiceId(int custType, string _voucherInvoiceId)
        {
            BO_ReceiptVoucher obj = new BussinessObject.BO_ReceiptVoucher();
            obj = BL_ReceiptVoucher.GetReceiptVoucherByInvoiceId(custType, _voucherInvoiceId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}