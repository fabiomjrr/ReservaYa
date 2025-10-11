using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ReservaYa
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "GestionEspcs",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "GestionEspacios", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EspaciosDT",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "EspaciosDetalles", action = "Mostrar", id = UrlParameter.Optional }
            );
        }
    }
}
