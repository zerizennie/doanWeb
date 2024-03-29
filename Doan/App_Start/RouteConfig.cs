﻿using System.Web.Mvc;
using System.Web.Routing;

namespace Doan
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}",
            new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Doan.Controllers" }
            );
            routes.MapRoute(
                name: "CateProduct",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "CateProduct", id = UrlParameter.Optional },
                namespaces: new[] { "Doan.Controllers" }

            );
            routes.MapRoute(
               name: "FindProduct",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "ResultFind", id = UrlParameter.Optional },
               namespaces: new[] { "Doan.Controllers" }

           );
        }
    }
}
