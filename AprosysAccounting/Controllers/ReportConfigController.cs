using AprosysAccounting.BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprosysAccounting.Controllers
{
    public class ReportConfigController : Appcode.BaseAprosysAccountingController
    {
        // GET: ReportConfig
        public ActionResult Index()
        {
            return View();
        }

        public string SaveReportConfig(string repoParam) {
            var obj = BussinessLogics.BL_Common.Deserialize<BO_RepoConfig>(repoParam);
            obj.empID = UserAprosysAccounting.id;
            return BussinessLogics.BL_Common.Serialize(BussinessLogics.BL_RepoConfig.SaveItem(obj));
        }
        public ActionResult GetReportConfig()
        {
            BO_RepoConfig obj = new BussinessObject.BO_RepoConfig();
            obj = BussinessLogics.BL_RepoConfig.GetReportConfig();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}