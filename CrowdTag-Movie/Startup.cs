using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CrowdTagMovie.Startup))]
namespace CrowdTagMovie
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
