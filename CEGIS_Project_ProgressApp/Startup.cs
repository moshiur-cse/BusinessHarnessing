using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CEGIS_Project_ProgressApp.Startup))]
namespace CEGIS_Project_ProgressApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
