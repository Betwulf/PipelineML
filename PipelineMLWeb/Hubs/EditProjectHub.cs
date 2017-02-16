using Microsoft.AspNet.SignalR;
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
        public void CreatePipelinePart(CreatePipelinePartViewModel createPart)
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
                        case 5:
                            Console.WriteLine(5);
                            break;
                    }
                    DbContext.SavePipelineDefinition(def.Id, def);
                }
            }
            // TODO: create type and add it to the appropriate project, then save and update project in UI
        }
    }
}
