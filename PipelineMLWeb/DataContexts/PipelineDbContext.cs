﻿using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
using Ninject;
using PipelineMLCore;
using Serilog;
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

        public DbSet<ClassSchema> ClassSchemas { get; set; }


        // find or create and save the schema for a class
        public ClassSchema GetClassSchema(Type classType, string title)
        {
            string classTypeString = classType.ToString();
            var cachedSchema = ClassSchemas.FirstOrDefault(x => x.Classname == classTypeString);
            if (cachedSchema != null)
            {
                return cachedSchema;
            }
            // otherwise make a new one
            var newSchema = new ClassSchema() { Classname = classTypeString };
            JSchemaGenerator generator = new JSchemaGenerator();
            generator.ContractResolver = new PipelinePartContractResolver();
            // types with no defined ID have their type name as the ID
            generator.SchemaIdGenerationHandling = SchemaIdGenerationHandling.TypeName;
            generator.DefaultRequired = Newtonsoft.Json.Required.Always;
            string partConfigSchema = generator.Generate(classType).ToString();
            partConfigSchema = partConfigSchema.Insert(1, $"\"title\": \"Edit: {title}\", ");
            partConfigSchema = partConfigSchema.Replace("$ref\": \"", "$ref\": \"#/definitions/");

            newSchema.Schema = partConfigSchema;
            ClassSchemas.Add(newSchema);
            SaveChangesAsync(); // no need to wait, failure IS an option
            return newSchema;
        }


        // The file storage portion of this database context
        public PipelineResults GetPipelineResultsByGuid(Guid id)
        {
            return Get<PipelineResults>(id);
        }
        public void SavePipelineResults(Guid id, PipelineResults results)
        {
            Save(id, results);
        }





        public PipelineDefinition GetPipelineDefinitionByGuid(Guid id)
        {
            var def = Get<PipelineDefinition>(id);
            def.Configure(_kernel);
            return def;
        }
        public void SavePipelineDefinition(Guid id, PipelineDefinition def)
        {
            Save(id, def);
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
            try
            {
                var json = _storage.ReadData(typeof(T).Name, id.ToString());
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"Error reading file: {typeof(T).Name} id: {id} Message: {ex.Message}");
            }
            return default(T);
        }

        private void Save<T>(Guid id, T dataObject)
        {
            var json = JsonConvert.SerializeObject(dataObject);
            _storage.WriteData(typeof(T).Name, id.ToString(), json);
        }

    }
}