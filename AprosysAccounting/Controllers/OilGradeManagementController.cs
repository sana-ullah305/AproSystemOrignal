using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class OilGradeManagementController : Appcode.BaseAprosysAccountingController
    {
        // GET: OilGradeManagement
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadOilGradeTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_OilGradeManagement.LoadOilGradeTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteGradeRecord(int Id)
        {
            
            return Json(BL_OilGradeManagement.DeleteGradeRecord(Id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveGrade(int? Id, string Grade)
        {

            return Json(BL_OilGradeManagement.SaveGrade(Id,Grade), JsonRequestBehavior.AllowGet);
        }
    }
}