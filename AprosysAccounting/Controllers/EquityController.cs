using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;

namespace AprosysAccounting.Controllers
{
    public class EquityController : Appcode.BaseAprosysAccountingController
    {
        // GET: Equity
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetShareHoldersName()
        {

            var List = BL_Equity.GetShareHoldersName();
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public string InsertEquityInfo(string param)
        {
            var obj = BL_Common.Deserialize<BO_Equity>(param);
            obj.userId = UserAprosysAccounting.id;
            if (!Config.AllowEditDate)
            {
                obj.activityTime = DateTime.Now;
            }
           
            return BL_Common.Serialize(BL_Equity.InsertEquityInfo(obj));
        }
        public string SaveInvestor(string paraminvestor)
        {
            var des = BL_Common.Deserialize<BO_CapitalManagement>(paraminvestor);
            return BL_Common.Serialize(BL_Equity.SaveInvestor(des.investorId, des.investorName, des.amount)); ;
        }

        public string DeleteInvestor(int CoaId)
        {
            return BL_Common.Serialize(BL_Equity.DeleteInvestor(CoaId)); ;
        }
        //public ActionResult CapitalManagement()
        //{
        //    return View();
        //}
        public ActionResult InvestorManagement()
        {
            return View();
        }

        public string GetInvestorList()
        {
            
            return BL_Common.Serialize(BL_Equity.GetInvestorList());
        }
        public ActionResult LoadEquityInfoTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_Equity.LoadEquityInfoTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }
        public string DeleteEquityInvoice(string invoiceNo)
        {
            var pl = BL_Equity.DeleteEquityInvoice(invoiceNo, UserAprosysAccounting.id);
            return BL_Common.Serialize("success");
        }
    }
}