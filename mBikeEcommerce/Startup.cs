using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mBikeEcommerce.Startup))]
namespace mBikeEcommerce
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
