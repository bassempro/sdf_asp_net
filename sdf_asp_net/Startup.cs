using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(sdf_asp_net.Startup))]
namespace sdf_asp_net
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
