using Microsoft.AspNet.SignalR;
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
            var projectClaim = currentUser.Claims.FirstOrDefault(x => x.ClaimType == PipelineClaimsTypes.PipelineProject && x.ClaimValue == guid);
            Guid tempGuid;
            if (Guid.TryParse(guid, out tempGuid))
            {
                if (projectClaim != null)
                {
                    var project = DbContext.Projects.FirstOrDefault(x => x.Id == tempGuid);
                    ProjectViewModel model = new ProjectViewModel(project);
                    var def = DbContext.GetPipelineDefinitionByGuid(project.PipelineDefinitionGuid);
                    if (def != null)
                    {
                        model.SetDefinition(def);
                        Clients.Caller.OnGetProject(model);
                    }
                    //TODO: Handle error case where def == null
                }
            }
        }
        
    }
}
