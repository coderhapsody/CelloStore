using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CelloStore.FrontEnd
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Article",
                url: "Pages/{urlToken}",
                defaults: new { controller = "Article", action = "ViewContent" },
                namespaces: new[] { "CelloStore.FrontEnd.Controllers" }
            );

            routes.MapRoute(
                name: "Catalog",
                url: "Shopping/Catalog/{areaName}/{categoryName}",
                defaults: new
                {
                    controller = "Shopping",
                    action = "Catalog",
                    categoryName = UrlParameter.Optional
                }
                );

            routes.MapRoute(
                name: "ProductDetail",
                url: "Shopping/ProductDetail/{areaName}/{id}",
                defaults: new
                {
                    controller = "Shopping",
                    action = "ProductDetail"
                }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CelloStore.FrontEnd.Controllers" }
            );


        }
    }
}
