using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Newtonsoft.Json;
using Ninject;
using PipelineMLCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace PipelineMLWeb.DataContexts
{
    public class MigrationsContextFactory : IDbContextFactory<PipelineDbContext>
    {
        public PipelineDbContext Create()
        {
            return new PipelineDbContext(Startup.CreateNinject());
        }
    }


    public class PipelineDbContext : DbContext
    {

        // The Entity Framework portion of this database context
        public DbSet<PipelineProject> Projects { get; set; }

        public DbSet<PipelineResultsId> RunResultIds { get; set; }



        // The file storage portion of this database context
        public PipelineResults GetPipelineResultsByGuid(Guid id)
        {
            return Get<PipelineResults>(id);
        }




        private IKernel _kernel;
        private IStorage _storage;

        public static PipelineDbContext Create(IdentityFactoryOptions<PipelineDbContext> options, IOwinContext context)
        {
            return new PipelineDbContext(context.Get<IKernel>());
        }

        public PipelineDbContext(IKernel kernel) : base("DefaultConnection")
        {
            _kernel = kernel;
            _storage = kernel.Get<IStorage>();
        }

        private T Get<T>(Guid id)
        {
            var json = _storage.ReadData(nameof(T), id.ToString());
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}