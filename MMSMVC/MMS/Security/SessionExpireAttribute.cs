using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MMS.Security
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            string redirect;

          
            // check  sessions here
            if (HttpContext.Current.Session["LoggedUserId"] == null)
            {

                redirect = string.Format("~/Account/Login?ReturnUrl={0}", HttpUtility.UrlEncode(ctx.Request.RawUrl));

                filterContext.Result = new RedirectResult(redirect);
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}