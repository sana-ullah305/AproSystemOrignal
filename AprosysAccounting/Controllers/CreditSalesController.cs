using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class CreditSalesController : Appcode.BaseAprosysAccountingController
    {
        // GET: CreditSales
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAllBankNames()
        {
            return Json(BL_BanksConfiguration.GetAllBankNames(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUnPaidCreditSalesList(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_CreditSales.GetUnPaidCreditSalesList(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUnPaidCreditCustomerList()
        {

            var List = BL_CreditSales.GetUnPaidCreditCustomerList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnPaidCreditCustomerInvoices(int custID)
        {

            var List = BL_CreditSales.GetUnPaidCreditCustomerInvoices(custID);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        
        public string UpdateUnPaidCreditSales(string paramcustomer)
        {

            var obj = BL_Common.Deserialize<BO_CreditSalesUpdate>(paramcustomer);
            if (!Config.AllowEditDate)
            {
                obj.creditPaidDate = DateTime.Now;
            }
            return BL_Common.Serialize((BL_CreditSales.UpdateUnPaidCreditSales(obj, UserAprosysAccounting.id)));

        }


    }
}