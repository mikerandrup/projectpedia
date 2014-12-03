using System.Web.Mvc;

namespace ProjectPediaWebAPI.Controllers
{
    public class AllowCrossOriginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            base.OnActionExecuted(filterContext);
        }
    }
}