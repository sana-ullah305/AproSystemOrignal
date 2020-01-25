using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using ApprosysAccDB;
using AprosysAccounting.Appcode;

namespace AprosysAccounting.Controllers
{
    public class PermissionAPIController : BaseAprosysAccountingController
    {
        // GET: PermissionAPI
        public ActionResult GetGlobalUserInfo()
        {
            var p = PermissionModal.GetUserDetail(UserAprosysAccounting.id);
            return Json(p, JsonRequestBehavior.AllowGet);
        }
    }
}