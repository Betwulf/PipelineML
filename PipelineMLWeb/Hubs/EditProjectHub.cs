using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Schema.Generation;
using PipelineMLCore;
using PipelineMLWeb.DataContexts;
using PipelineMLWeb.Models;
using System;
using System.Linq;

namespace PipelineMLWeb.Hubs
{
    [Authorize]
    public class EditProjectHub : Hub
    {
        [Authorize]
        public void GetProject(string guid)
        {
            ApplicationUser currentUser = this.GetApplicationUser();
            var DbContext = this.GetPipelineDbContext();
            var project = GetProjectInternal(currentUser, DbContext, guid);
            if (project != null)
            {
                ProjectViewModel model = new ProjectViewModel(project);
                var def = DbContext.GetPipelineDefinitionByGuid(project.PipelineDefinitionGuid);
                if (def != null)
                {
                    model.SetDefinition(def);
                    Clients.Caller.OnGetProject(model);
                }
                //TODO: Handle error case where def == null
            }
            // TODO: Handle case where project is null
        }


        private PipelineProject GetProjectInternal(ApplicationUser user, PipelineDbContext dbContext, string projectId)
        {
            var projectClaim = user.Claims.FirstOrDefault(x => x.ClaimType == PipelineClaimsTypes.PipelineProject && x.ClaimValue == projectId);
            Guid tempGuid;
            if (Guid.TryParse(projectId, out tempGuid))
            {
                if (projectClaim != null)
                {
                    var project = dbContext.Projects.FirstOrDefault(x => x.Id == tempGuid);
                    return project;
                }
            }
            return null;
        }


        [Authorize]
        public void CreatePipelinePart(EditPipelinePartViewModel createPart)
        {
            ApplicationUser currentUser = this.GetApplicationUser();
            var DbContext = this.GetPipelineDbContext();
            Type classType = Type.GetType(createPart.classType);
            var project = GetProjectInternal(currentUser, DbContext, createPart.projectId);
            if (project != null)
            {
                var def = DbContext.GetPipelineDefinitionByGuid(project.PipelineDefinitionGuid);
                IPipelinePart part = (IPipelinePart)Activator.CreateInstance(classType);
                // TODO: Part Guid not being created?
                if (def != null)
                {
                    switch (createPart.columnNumber)
                    {
                        case 0:
                            // DataGenerator
                            Console.WriteLine($"Creating DatasetGenerator: {createPart.classType}");
                            def.DatasetGenerator = new TypeDefinition() { ClassType = classType, ClassConfig = part.Config.ToJSON() };
                        break;
                        case 1:
                            // Preprocess data transform
                            Console.WriteLine($"Creating pre process transform: {createPart.classType}");
                            def.PreprocessDataTransforms.Add(new TypeDefinition() { ClassType = classType, ClassConfig = part.Config.ToJSON() });
                            break;
                        case 2:
                            // ML
                            Console.WriteLine($"Creating machine learning: {createPart.classType}");
                            def.MLList.Add(new TypeDefinition() { ClassType = classType, ClassConfig = part.Config.ToJSON() });
                            break;
                        case 3:
                            // Postprocess data transform
                            Console.WriteLine($"Creating post process transform: {createPart.classType}");
                            def.PostprocessDataTransforms.Add(new TypeDefinition() { ClassType = classType, ClassConfig = part.Config.ToJSON() });
                            break;
                        case 4:
                            // evaluator
                            Console.WriteLine($"Creating evaluator: {createPart.classType}");
                            def.Evaluators.Add(new TypeDefinition() { ClassType = classType, ClassConfig = part.Config.ToJSON() });
                            break;
                        default:
                            Console.WriteLine($"unknown column number {createPart.columnNumber} for {createPart.classType} ");
                            //TODO: Handle this case better
                            break;
                    }
                    DbContext.SavePipelineDefinition(def.Id, def);
                    // Return updated project  to UI
                    ProjectViewModel model = new ProjectViewModel(project);
                    model.SetDefinition(def);
                    Clients.Caller.OnGetProject(model);
                }
            }
            // TODO: create type and add it to the appropriate project, then save and update project in UI
        }


        [Authorize]
        public void GetPipelinePartAndSchema(EditPipelinePartViewModel partData)
        {
            try
            {
                ApplicationUser currentUser = this.GetApplicationUser();
                var DbContext = this.GetPipelineDbContext();
                Type classType = Type.GetType(partData.classType);
                var project = GetProjectInternal(currentUser, DbContext, partData.projectId);
                if (project != null)
                {
                    var def = DbContext.GetPipelineDefinitionByGuid(project.PipelineDefinitionGuid);
                    var part = def.CreateInstanceOf(partData.columnNumber, classType, partData.pipelinePartId);
                    string partConfigJson = part.Config.ToJSON();

                    JSchemaGenerator generator = new JSchemaGenerator();
                    generator.ContractResolver = new PipelinePartContractResolver();
                    // types with no defined ID have their type name as the ID
                    generator.SchemaIdGenerationHandling = SchemaIdGenerationHandling.TypeName;
                    generator.DefaultRequired = Newtonsoft.Json.Required.Always;
                    string partConfigSchema = generator.Generate(part.Config.GetType()).ToString();
                    partConfigSchema = partConfigSchema.Insert(1, $"\"title\": \"Edit: {part.Name}\", ");

                    var data = new PipelinePartSchemaAndData()
                    {
                        classType = partData.classType,
                        columnNumber = partData.columnNumber,
                        projectId = partData.projectId,
                        pipelinePartId = partData.pipelinePartId
                    };
                    data.schemaJSON = partConfigSchema;
                    data.dataJSON = partConfigJson;

                    Clients.Caller.OnEditPipelinePart(data);


                }
            }
            catch (Exception ex)
            {
                var errorData = new ErrorViewModel();
                errorData.FriendlyMessage = "There was an error trying to edit your project.";
                errorData.ErrorMessage = ex.Message;
                Clients.Caller.OnError(errorData);
            }
        }
    }
}
