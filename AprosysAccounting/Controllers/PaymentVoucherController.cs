using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class PaymentVoucherController : Appcode.BaseAprosysAccountingController
    {
        // GET: PaymentVoucher
        public ActionResult Index()
        {
            return View();
        }
        //public string GetNextVoucher()
        //{
        //    string voucher = BL_Common.GetLastVoucher(4);
        //    if (String.IsNullOrEmpty(voucher)) { return BL_Common.Serialize("PMT-1"); }
        //    var increment = Convert.ToInt32(voucher.Split('-')[1]) + 1;
        //    return BL_Common.Serialize("PMT-" + increment);
        //}
        public JsonResult GetExpenseTypeList()
        {

            var List = BL_PaymentVoucher.GetPaymentVoucherExpenseTypeList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExpense(int coaID)
        {

            var List = BL_PaymentVoucher.GetExpense(coaID);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadPaymentVoucherTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_PaymentVoucher.LoadPaymentVoucherTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }
        public string SavePaymentVoucher(string pvoucher)
        {

           
                var Pvoucher = BL_Common.Deserialize<BO_PaymentVoucher>(pvoucher);
                Pvoucher.empID = UserAprosysAccounting.id;
                if (!Config.AllowEditDate)
                {
                    Pvoucher.activityDate = DateTime.Now;
                }
                return BL_Common.Serialize((BL_PaymentVoucher.SavePaymentVoucher(Pvoucher)));
          
        }
        public string DeletePaymentVoucher(string invoiceNo)
        {
            var pl = BL_PaymentVoucher.DeletePaymentVoucher(invoiceNo,UserAprosysAccounting.id);
            return BL_Common.Serialize("success");
        }
        public ActionResult GetPaymentVoucherByInvoiceId(string _voucherInvoiceId)
        {
            BO_PaymentVoucher obj = new BussinessObject.BO_PaymentVoucher();
            obj = BL_PaymentVoucher.GetPaymentVoucherByInvoiceId(_voucherInvoiceId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public string EditPaymentVoucher(string pvoucher)
        {
            var Pvoucher = BL_Common.Deserialize<BO_PaymentVoucher>(pvoucher);
            Pvoucher.empID = UserAprosysAccounting.id;
            return BL_Common.Serialize((BL_PaymentVoucher.EditPaymentVoucher(Pvoucher)));

        }

    }
}