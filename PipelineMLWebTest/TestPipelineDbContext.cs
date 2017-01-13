using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineMLCore;
using PipelineMLWeb.DataContexts;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;

namespace PipelineMLWebTest
{
    [TestClass]
    public class TestPipelineDbContext
    {
        [TestMethod]
        public void ProperDataSeedingInMigration()
        {
            try
            {
                MigrationsContextFactory factory = new MigrationsContextFactory();
                var context = factory.Create();
                var pguid = Guid.Parse("00000000-0000-0000-0000-000000000000");
                context.Projects.AddOrUpdate(new PipelineProject() { Id = pguid, Name = "Test", Description = "Testing", PipelineDefinitionGuid = pguid });
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


        }
    }
}
