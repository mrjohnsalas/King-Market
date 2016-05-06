using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(King_Market.Startup))]
namespace King_Market
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
