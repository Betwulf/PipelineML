using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Ninject;
using Owin;
using PipelineMLCore;
using PipelineMLWeb.DataContexts;
using Serilog;
using System;

[assembly: OwinStartupAttribute(typeof(PipelineMLWeb.Startup))]
namespace PipelineMLWeb
{
    public partial class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
            Log.Logger.Information("Application Startup {time}", DateTime.Now);

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
