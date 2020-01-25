using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AprosysAccounting.BussinessObject;
using AprosysAccounting.BussinessLogics;

namespace AprosysAccounting.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            string UserName = Request["username"];
            string Password = Request["password"];

            Constants.LoginResult result;
            BO_Users user = new BO_Users();
            BO_Users currentUser = BL_Users.Login(Session, UserName, Password, out result);

            if (UserName != null && Password != null)
            {
                if (result == Constants.LoginResult.Success)
                {
                    Session[Constants.SESSION_USERKEY] = currentUser;
                    return Redirect("/Sales/Index");
                }
                else
                {
                    ViewBag.Validation = "Invalid";
                }
            }

           

            return View();
        }

        public ActionResult Logout(string redirecturl)
        {
            return Redirect("/Admin/Index");
        }

        public string ResetGL()
        {
            var pl = BL_Admin.ResetGL();
            return BL_Common.Serialize("success");
        }
    }
}