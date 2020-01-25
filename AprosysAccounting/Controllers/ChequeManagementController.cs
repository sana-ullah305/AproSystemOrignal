using AprosysAccounting.Appcode;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class ChequeManagementController : BaseAprosysAccountingController
    {
        // GET: ChequeManagement
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// outStanding cheque = Cheque Not Deposited
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public ActionResult LoadOutStandingChequeTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_ChequeManagement.LoadOutStandingChequeTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnSubmitDocumentList()
        {

            var List = BL_ChequeManagement.LoadOutStandingCheque(null);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

    }
}