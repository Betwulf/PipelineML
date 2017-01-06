using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;
using Ninject;
using PipelineMLCore;

namespace PipelineML
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IStorage>().To<StorageFile>();

            Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
            Log.Logger.Information("Application Startup {time}", DateTime.Now);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(kernel));

        }

    }
}
