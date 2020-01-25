using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;

namespace AprosysAccounting.Controllers
{
    public class UserController : Appcode.BaseAprosysAccountingController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public string SaveUser(string paramuser)
        {
       
            var _user = BL_Common.Deserialize<BO_Users>(paramuser);
            return BL_Common.Serialize(BL_Users.SaveUser(_user, UserAprosysAccounting.id));
        }

        public ActionResult LoadUserTable(JQueryDataTableParamModel Param)
        {
            MYJSONTblCustom MYJSON = BL_Users.LoadUserTable(Param, Request);
            return Json(MYJSON, JsonRequestBehavior.AllowGet);
        }

        public string DeleteUser(int id)
        {
            var pl = BL_Users.DeleteUser(id, UserAprosysAccounting.id);
            return BL_Common.Serialize("success");
        }

        public ActionResult GetUserByID(int  _userId)
        {
            BO_Users obj = new BussinessObject.BO_Users();
            obj = BL_Users.GetUserByID(_userId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public string SendTestEmail()
        {
            try
            {
                Appcode.Email.sendEmail("TESTEMAILAPROSYS", "TESTEMAILAPROSYS");
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}