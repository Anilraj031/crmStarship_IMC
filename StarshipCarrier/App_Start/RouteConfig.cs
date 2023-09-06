using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace StarshipCarrier.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("default", "", "~/WebForm1.aspx");
            routes.MapPageRoute("default1", "getCarrier/{Quotenumber}/{Location}/{Address1}/{Address2}/{City}/{State}/{Zipcode}/{Weight}", "~/WebForm1.aspx");
        }
    }
}