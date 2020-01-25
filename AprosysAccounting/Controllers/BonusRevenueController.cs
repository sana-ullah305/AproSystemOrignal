using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class BonusRevenueController : Appcode.BaseAprosysAccountingController
    {
        // GET: BonusRevenue
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBonusRevenueList(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_BonusRevenue.GetBonusRevenueList(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }
        public string GetRevenueAccountList()
        {
            return BL_Common.Serialize(BL_BonusRevenue.GetRevenueAccountList());

        }
        public string GetBankList()
        {
            return BL_Common.Serialize(BL_BonusRevenue.GetBankList());

        }
        public string SaveBonus(BO_BonusRevenue br)
        {
            var empID = UserAprosysAccounting.id;
            return BL_Common.Serialize(BL_BonusRevenue.SaveBonus(br, empID));

        }

        public JsonResult GetBonusRecordById(int transId)
        {

            var List = BL_BonusRevenue.GetBonusRecordById(transId);
            return Json(List, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteBonus(int transId)
        {

            var List = BL_BonusRevenue.DeleteBonus(transId);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

    }
}