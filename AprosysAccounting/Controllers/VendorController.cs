using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;

namespace AprosysAccounting.Controllers
{
    public class VendorController : Appcode.BaseAprosysAccountingController
    {
        // GET: Vendor
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetVendorList()
        {

            var List = BL_Vendor.GetVendorList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadVendorTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_Vendor.LoadVendorTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }
        public string SaveVendor(string paramVendor)
        {

            var _vendor = BL_Common.Deserialize<BO_Vendor>(paramVendor);
            return BL_Common.Serialize((BL_Vendor.SaveVendor(_vendor,UserAprosysAccounting.id)));

        }
        public string DeleteVendor(int vendorID)
        {
            var pl = BL_Vendor.DeleteVendor(vendorID, UserAprosysAccounting.id);
            return BL_Common.Serialize(pl);
        }
        public string GetVendorBalance(int vendID)
        {
            var pl = BL_Vendor.GetVendorBalance(vendID);
            return BL_Common.Serialize(pl);
        }
        public ActionResult GetVendorByID(int vendorID)
        {
            BO_Vendor obj = new BussinessObject.BO_Vendor();
            obj = BL_Vendor.GetVendorByID(vendorID);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}