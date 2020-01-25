using System;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class DailyCashFlowController : Appcode.BaseAprosysAccountingController
    {
        // GET: DailyCashFlow
        public ActionResult Index()
        {
            return View();
        }

        public string GetDailyCashFlow(string date)
        {
            DateTime datex = Convert.ToDateTime(date);
            return BussinessLogics.BL_Common.Serialize(BussinessLogics.BL_DailyCashFlow.GetDailyCashFlow(datex));
        }
    }
}