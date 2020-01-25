using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AprosysAccounting.Appcode;
using AprosysAccounting.BussinessObject;
using AprosysAccounting.BussinessLogics;

namespace AprosysAccounting.Controllers
{
    public class SalesPersonManagementController : BaseAprosysAccountingController
    {
        // GET: SalesPersonManagement
        public ActionResult Index()
        {
            return View();
        }
        public string GetCustomersList()
        {
            return BL_Common.Serialize(BL_SalesPersonManagement.GetCustomersList());
        }
        public string GetCustomersListBySalePersonID(int salePersonID)
        {
            return BL_Common.Serialize(BL_SalesPersonManagement.GetCustomersListBySalePersonID(salePersonID));
        }
        public string SaveSalesPerson(string paramcustomer)
        {

            var _customer = BL_Common.Deserialize<BO_SalesPersonManagement>(paramcustomer);
            return BL_Common.Serialize((BL_SalesPersonManagement.SaveSalesPerson(_customer, UserAprosysAccounting.id)));

        }

        public ActionResult LoadSalesPersonTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_SalesPersonManagement.LoadSalesPersonTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSalesPersonByID(int salePersonID)
        {
            BO_SalesPersonManagement obj = new BussinessObject.BO_SalesPersonManagement();
            obj = BL_SalesPersonManagement.GetSalesPersonByID(salePersonID);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public string DeleteSalesPerson(int salesPersonID)
        {
            var pl = BL_SalesPersonManagement.DeleteSalesPerson(salesPersonID, UserAprosysAccounting.id);
            return BL_Common.Serialize(pl);
        }
    }
}
