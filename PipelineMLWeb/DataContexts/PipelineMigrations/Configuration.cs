namespace PipelineMLWeb.DataContexts.PipelineMigrations
{
    using PipelineMLCore;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PipelineMLWeb.DataContexts.PipelineDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataContexts\PipelineMigrations";
        }

        protected override void Seed(PipelineMLWeb.DataContexts.PipelineDbContext context)
        {
            var pguid = Guid.Parse("00000000-0000-0000-0000-000000000000");
            context.Projects.AddOrUpdate(new PipelineProject() { Id = pguid, Name = "Test", Description = "Test", PipelineDefinitionGuid = pguid });
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
