using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;


namespace AprosysAccounting.Controllers
{
    public class SubscriptionManagmentController : Appcode.BaseAprosysAccountingController
    {
        // GET: SubscriptionManagment
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadSubscriptionTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_Subscription.LoadSubscriptionTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReminderLog(int subCustId)
        {
            var log = BL_Subscription.GetReminderLogs(subCustId);
            return Json(log, JsonRequestBehavior.AllowGet);
        }
        public string SaveSubscriptionCustomer(string paramcustomer)
        {
            //try catch
            var _customer = BL_Common.Deserialize<BO_SubCustomer>(paramcustomer);
            return BL_Common.Serialize((BL_Subscription.SaveSubscriptionCustomer(_customer, UserAprosysAccounting.id)));
        }

        public string EditSubscriptionCustomer(string paramcustomer)
        {
            //try catch
            var _customer = BL_Common.Deserialize<BO_SubCustomer>(paramcustomer);
            return BL_Common.Serialize((BL_Subscription.EditSubscriptionCustomer(_customer, UserAprosysAccounting.id)));
        }
        public string DeleteSubscriptionCustomer(int subcustomerID)
        {
            var pl = BL_Subscription.DeleteSubscriptionCustomer(subcustomerID);
            return BL_Common.Serialize(pl);
        }

        public ActionResult GetSubCustByID(int subCustId)
        {
            BO_SubCustomer obj = new BussinessObject.BO_SubCustomer();
            obj = BL_Subscription.GetSubCustByID(subCustId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public string SaveSuspendedDuesofCustomer(int SubCustomerId)
        {
            //try catch
            BL_Subscription obj = new BussinessLogics.BL_Subscription();

            return BL_Common.Serialize(obj.ClearSuspendedDuesofCustomer(SubCustomerId));
            //return BL_Common.Serialize(BL_ReceiptVoucher.SaveSuspendedDuesofCustomer(customerId,UserAprosysAccounting.id));
        }
        //public string ValidateDueDates(string actDate)
        //{
        //    DateTime date = String.IsNullOrEmpty(actDate) ? DateTime.Now.Date : Convert.ToDateTime(actDate);
        //    return BL_SubscriptionManagement.ValidateDueDates(date);
        //}
    }
}