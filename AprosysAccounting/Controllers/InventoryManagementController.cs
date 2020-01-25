using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;

namespace AprosysAccounting.Controllers
{
    public class InventoryManagementController : Appcode.BaseAprosysAccountingController
    {
        // GET: InventoryManagement
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadInventoryTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_Inventory.LoadInventoryTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }
    }
}