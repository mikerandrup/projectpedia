using System.Data.SqlClient;
using System.Web.Mvc;
using ProjectPediaWebAPI.PortfolioCore;

namespace ProjectPediaWebAPI.Controllers
{
    public class DatabaseController : Controller
    {
        protected bool NeedsDBConnection = true;
        protected SqlConnection _ConnectionToDB { get; set; }

        override protected void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (NeedsDBConnection)
            {
                _ConnectionToDB = Connection.Create();
                _ConnectionToDB.Open();
            }
        }

        override protected void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (NeedsDBConnection)
            {
                _ConnectionToDB.Close();
            }
            base.OnActionExecuted(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (NeedsDBConnection)
            {
                if (_ConnectionToDB != null) 
                    _ConnectionToDB.Close();
            }
            base.OnException(filterContext);
        }

    }
}
