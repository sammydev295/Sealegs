using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Sealegs.Backend.Startup))]

namespace Sealegs.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}