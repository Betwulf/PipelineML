using Ninject.Activation;
using PipelineMLCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace PipelineMLWeb
{
    // This class gives Ninject a provider interface to generate a storage object that has been properly configured
    public class StorageProvider : Provider<IStorage>
    {
        public StorageProvider(NameValueCollection appSettings)
        {
            _appSettings = appSettings;
        }

        public NameValueCollection _appSettings { get; private set; }

        protected override IStorage CreateInstance(IContext context)
        {
            try
            {
                bool useFiles = bool.Parse(_appSettings["IStorage:UseFiles"]);
                string path = _appSettings["IStorage:Filepath"];

                if (useFiles)
                {
                    var storage = new StorageFile(path);
                    return storage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Possible missing configuration in web.config");
                Console.WriteLine(ex.Message);
            }
            // TODO: Handle other storage types.
            throw new NotImplementedException("Did not handle any other storage implementation other than file yet.");
        }

    }
}