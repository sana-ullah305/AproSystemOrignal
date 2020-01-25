using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using AprosysAccounting.Appcode;

namespace AprosysAccounting.Controllers
{
    public class ServiceController : Appcode.BaseAprosysAccountingController
    {
        // GET: Service
        public ActionResult Index()
        {
            return View();
        }
        public string SaveRevenueSales(string name,decimal cost,string code)
        {

            //  var Pvoucher = BL_Common.Deserialize<BO_COA>(pvoucher);
            return BL_Common.Serialize((BL_Service.SaveRevenueSales(name, cost,code)));

        }
        public ActionResult LoadCustomerTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_Service.LoadServiceTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }

        public string EditRevenueSales(int id, string name,decimal cost,string code)
        {

            //  var Pvoucher = BL_Common.Deserialize<BO_COA>(pvoucher);
            return BL_Common.Serialize((BL_Service.EditRevenueSales(id,name,cost,code)));

        }

        public ActionResult GetServiceByID(int id)
        {
            BO_Service obj = new BussinessObject.BO_Service();
            obj = BL_Service.GetServiceByID(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public string DeleteRevenueService(int id)
        {
            return BL_Common.Serialize((BL_Service.DeleteRevenueService(id)));

        }
    }
}