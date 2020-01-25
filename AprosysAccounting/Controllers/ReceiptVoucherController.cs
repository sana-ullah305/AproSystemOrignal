using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class ReceiptVoucherController : Appcode.BaseAprosysAccountingController
    {
        // GET: ReceiptVoucher
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
            MYJSONTblCustom MYJSON = BL_ReceiptVoucher.LoadReceiptVoucherTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }
        public string SaveReceiptVoucher(string rvoucher)
        {
            var _rvoucher = BL_Common.Deserialize<BO_ReceiptVoucher>(rvoucher);
            _rvoucher.empID = UserAprosysAccounting.id;
            if (!Config.AllowEditDate)
            {
                _rvoucher.rActivityDate = DateTime.Now;
            }
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

        public string EditReceiptVoucher(string rvoucher)
        {
            var _rvoucher = BL_Common.Deserialize<BO_ReceiptVoucher>(rvoucher);
            _rvoucher.empID = UserAprosysAccounting.id;
            return BL_Common.Serialize((BL_ReceiptVoucher.EditReceiptVoucher(_rvoucher)));
        }
    }
}