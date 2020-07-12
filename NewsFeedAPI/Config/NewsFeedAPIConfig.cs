﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace NewsFeedAPI.Config
{
    public class NewsFeedAPIConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            config.Routes.MapHttpRoute(
                name: "RatingRoute",
                routeTemplate: "api/{controller}/{action}/{id}/{rating}"
            );
            config.Routes.MapHttpRoute(
                name: "CancelRatingRoute",
                routeTemplate: "api/{controller}/{action}/{id}"
            );
        }
    }
}