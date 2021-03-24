using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SpeedyAPI.Controllers;
using SpeedyAPI.Extensions;
using SpeedyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedyAPI.Filters
{
    public class AdminFilter: Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var admin = context.HttpContext.Session.GetString(AdminController.SESSION_ADMIN_ROLE);
            if (admin == null)
            {
                context.Result = new RedirectResult("~/Admin");
            }
        }
    }
}
