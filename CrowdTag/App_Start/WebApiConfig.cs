using System.Web.Http;

namespace CrowdTagMovie
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

        }
    }
}
