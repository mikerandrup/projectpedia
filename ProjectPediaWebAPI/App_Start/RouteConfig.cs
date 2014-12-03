using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectPediaWebAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            string[] namespaces = new string[] { "ProjectPediaWebAPI.Controllers" };

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Browse",
                url: "",
                defaults: new { controller = "Home", action = "Browse" },
                namespaces: namespaces
            );

            routes.MapRoute(
                "Admin_ImportSQL",
                "admin/importsql",
                new { controller = "Admin", action = "ImportSql" },
                namespaces
            );

            routes.MapRoute(
                name: "HandleDataRequest",
                url: "api/{coreModuleName}/{id}",
                defaults: new {
                    controller = "CoreData", 
                    action = "FetchData", 
                    id = UrlParameter.Optional
                }
            );

        }
    }
}