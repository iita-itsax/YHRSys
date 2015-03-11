using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YHRSys.Startup))]
namespace YHRSys
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
