using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApplication23.Startup))]
namespace WebApplication23
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
