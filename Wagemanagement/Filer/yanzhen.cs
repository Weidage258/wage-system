using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wagemanagement.Filer
{
    public class yanzhen:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (HttpContext.Current.Session["Login"]==null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }
        }
    }
}