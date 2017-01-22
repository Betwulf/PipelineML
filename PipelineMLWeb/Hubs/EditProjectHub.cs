using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Ninject;
using PipelineMLCore;
using PipelineMLWeb.DataContexts;
using PipelineMLWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web;

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

        [Authorize]
        public void GetAvailableClassTypes()
        {
            try
            {
                PipelinePartListViewModel model = new PipelinePartListViewModel();

                Clients.Caller.OnGetAvailableClassTypes(model);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }

        }
    }
}