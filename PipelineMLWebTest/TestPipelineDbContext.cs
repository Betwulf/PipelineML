using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineMLCore;
using PipelineMLWeb.DataContexts;
using System.Data.Entity.Validation;
using System.Data.Entity.Migrations;
using PipelineMLWeb.Controllers;
using PipelineMLWeb.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.Diagnostics;

namespace PipelineMLWebTest
{
    [TestClass]
    public class TestPipelineDbContext
    {

        public Guid TestProjectGuid { get
            {
                return Guid.Parse("00000000-0000-0000-0000-000000000000");
            }
        }

        public Guid TestPipelineDefinitionGuid
        {
            get
            {
                return Guid.Parse("00000000-0000-0000-0000-000000000001");
            }
        }
        public string TestUsername
        {
            get
            {
                return "a@a.com";
            }
        }

        public string TestPassword
        {
            get
            {
                return "aQ!123456";
            }
        }


        [TestMethod]
        public void ProperDataSeedingInMigration()
        {
            try
            {
                MigrationsContextFactory factory = new MigrationsContextFactory();
                var context = factory.Create();
                context.Projects.AddOrUpdate(new PipelineProject() { Id = TestProjectGuid, Name = "Test", Description = "Testing", PipelineDefinitionGuid = TestPipelineDefinitionGuid });
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

         
        }


    }
}
