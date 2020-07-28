using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SCS.Startup))]
namespace SCS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
