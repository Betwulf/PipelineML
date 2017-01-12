using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Ninject;
using Owin;
using PipelineMLCore;
using PipelineMLWeb.DataContexts;

[assembly: OwinStartupAttribute(typeof(PipelineMLWeb.Startup))]
namespace PipelineMLWeb
{
    public partial class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureNinject(app);
            app.CreatePerOwinContext<PipelineDbContext>(PipelineDbContext.Create);
            app.MapSignalR();
        }

        public void ConfigureNinject(IAppBuilder app)
        {
            app.CreatePerOwinContext(CreateNinject);
        }


        public static IKernel CreateNinject()
        {
            IKernel _kernel = new StandardKernel();
            _kernel.Bind<IStorage>().To<StorageFile>();
            return _kernel;
        }
    }
}
