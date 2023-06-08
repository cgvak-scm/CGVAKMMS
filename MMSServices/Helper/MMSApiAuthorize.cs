using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc.Filters;

namespace MMSServices.Helper
{
    public class MMSApiAuthorize : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext context)
        {
            var headers = context.Request.Headers;
            string token = string.Empty;
            if (context.Request.Headers.Contains("Authorization"))
            {
                token = context.Request.Headers.GetValues("Authorization").First();
                var ApiKey = WebConfigurationManager.AppSettings["AuthorizationKey"];
                if(token==ApiKey)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
           else
            {
                return false;
            }
        }
    }
}