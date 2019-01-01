using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(E_Mechanik_Web.Startup))]
namespace E_Mechanik_Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
