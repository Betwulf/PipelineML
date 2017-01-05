using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PipelineMLWeb.Startup))]
namespace PipelineMLWeb
{
    public partial class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
