using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AprosysAccounting.BussinessLogics;
using AprosysAccounting.BussinessObject;
using System.Web.Routing;

namespace AprosysAccounting.Appcode
{
    [ReplaceTags]
    public class BaseAprosysAccountingController : Controller
    {
        // GET: BaseAprosysAccounting
        protected string RequestGUID { get; private set; } = "";
        public BO_Users UserAprosysAccounting { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            UserAprosysAccounting = requestContext.HttpContext.Session[BussinessLogics.Constants.SESSION_USERKEY] as BO_Users;
            RequestGUID = Guid.NewGuid().ToString();
            string IP = requestContext.HttpContext.Request.UserHostAddress;
            
            //if (!AllowedIPs.Contains(IP) && !requestContext.HttpContext.Request.IsLocal)
            //{
            //    UserRSRS = null;
            //}

            try
            {
                string sessionID = "NULL";
                if (requestContext.HttpContext.Session != null)
                {
                    sessionID = requestContext.HttpContext.Session.SessionID;
                }
                string LogMessage = "RequestGUID is " + RequestGUID + " IP is " + requestContext.HttpContext.Request.UserHostAddress + ",  UserID = " + UserAprosysAccounting.id + ", UserName=" + UserAprosysAccounting.firstName + " , SessionID is " + sessionID + " Url is " + requestContext.HttpContext.Request.RawUrl + " PostData is" + requestContext.HttpContext.Request.Form.ToString();
                Logger.Write("RequestLog", LogMessage, "", Logger.LogType.InformationLog);

            }
            catch
            {

            }
            if (UserAprosysAccounting == null)
            {
                string _actionName = requestContext.RouteData.Values["action"].ToString();
                UrlHelper Url = new UrlHelper(requestContext);
                if (_actionName == "Index" ||
                    _actionName == "Beta" //We Have to remove this consdition when we change Name of Home/Beta to Home/Index
                    )
                {
                    var currenturl = requestContext.HttpContext.Request.Url.AbsoluteUri;
                    requestContext.HttpContext.Response.Clear();
                    requestContext.HttpContext.Response.Redirect(Url.Action("Logout", "Admin", new { redirecturl = currenturl }));
                    requestContext.HttpContext.Response.End();
                }
                else
                {
                    //For Ajax Call's
                    requestContext.HttpContext.Response.Clear();
                    requestContext.HttpContext.Response.Redirect(Url.Action("GetSessionExpiredCode", "Admin"));
                    requestContext.HttpContext.Response.End();
                }
            }
            else
            {

                base.Initialize(requestContext);
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exp = filterContext.Exception;
            string exceptionstring = filterContext.Exception.ToString();
            var entityValiddation = exp as System.Data.Entity.Validation.DbEntityValidationException;
            if (entityValiddation != null)
            {
                

               exceptionstring += string.Join(Environment.NewLine, 
                   entityValiddation.EntityValidationErrors.Select(e =>
                    string.Join(Environment.NewLine, e.ValidationErrors.Select(v => 
                        string.Format("{0} - {1}", v.PropertyName, v.ErrorMessage)
                        )
                       )
                      )
                     );
            }
            Logger.Write("Error for GUID="+RequestGUID+" Url  " + filterContext.RequestContext.HttpContext.Request.Url,exceptionstring, "", Logger.LogType.ErrorLog);
           Email.sendEmailAsync("Error for GUID=" + RequestGUID + " Url " + filterContext.RequestContext.HttpContext.Request.Url + "Exception is " +exceptionstring, "APROSYS_ERROR");
            base.OnException(filterContext);
        }
    }
}