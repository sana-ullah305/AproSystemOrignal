using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprosysAccounting.Appcode
{
    public class ReplaceTagsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.ActionDescriptor.ActionName.ToLower() != "index") { return; }
            var response = filterContext.HttpContext.Response;
            response.Filter = new ReplaceTagsStream(response.Filter);
        }

    }
}