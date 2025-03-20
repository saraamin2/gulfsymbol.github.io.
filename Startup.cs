using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GulfSymbolProject.Startup))]
namespace GulfSymbolProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
