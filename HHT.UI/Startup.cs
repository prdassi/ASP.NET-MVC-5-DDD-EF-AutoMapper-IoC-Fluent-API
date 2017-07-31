using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HHT.UI.Startup))]
namespace HHT.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
