using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AprosysAccounting.Startup))]
namespace AprosysAccounting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
