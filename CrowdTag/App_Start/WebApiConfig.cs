﻿using System.Web.Http;

namespace CrowdTag
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

        }
    }
}
