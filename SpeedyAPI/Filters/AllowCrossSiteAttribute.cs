using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedyAPI.Filters
{
    public class AllowCrossSiteAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:18081");
            filterContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            filterContext.HttpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            filterContext.HttpContext.Response.Headers.Add("Vary", "Origin");

            base.OnActionExecuting(filterContext);
        }
    }
}
