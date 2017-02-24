using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Ninject;
using Owin;
using PipelineMLCore;
using PipelineMLWeb.DataContexts;
using Serilog;
using System;
using System.Collections.Specialized;
using System.Configuration;

[assembly: OwinStartup(typeof(PipelineMLWeb.Startup))]
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
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            IKernel _kernel = new StandardKernel();
            //_kernel.Bind<IStorage>().To<StorageFile>();
            _kernel.Bind<IStorage>().ToProvider(new StorageProvider(appSettings));
            return _kernel;
        }
    }
}
