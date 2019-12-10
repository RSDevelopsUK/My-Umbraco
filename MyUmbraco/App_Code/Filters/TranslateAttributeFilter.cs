using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace MyUmbraco.Filters
{
  public class TranslateAttributeFilter : ActionFilterAttribute
  {
    public override void OnResultExecuted(ResultExecutedContext filterContext)
    {
      //You may fetch data from database here 
      filterContext.Controller.ViewBag.GreetMesssage = "Hello Foo";
      base.OnResultExecuted(filterContext);
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      var controllerName = filterContext.RouteData.Values["controller"];
      var actionName = filterContext.RouteData.Values["action"];
      var message = $"{"onactionexecuting"} controller:{controllerName} action:{actionName}";
      Debug.WriteLine(message, "Action Filter Log");
      base.OnActionExecuting(filterContext);
    }
  }
}