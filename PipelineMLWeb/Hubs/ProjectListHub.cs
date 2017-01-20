using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Ninject;
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
    public class ProjectListHub : Hub
    {
        [Authorize]
        public void GetProjects()
        {
            try
            {
                // Get ninject, the current user, identity DB and pipeline DB
                ApplicationUser currentUser = this.GetApplicationUser();
                var DbContext = this.GetPipelineDbContext();


                // Find the current user's project claims, then pull up the associated projects
                var projectClaims = currentUser.Claims.Where(x => x.ClaimType == PipelineClaimsTypes.PipelineProject).Select(x => x.ClaimValue);
                List<ProjectViewModel> viewModel = new List<ProjectViewModel>();
                foreach (var aProjectGuid in projectClaims)
                {
                    Guid tempGuid;
                    if (Guid.TryParse(aProjectGuid, out tempGuid))
                    {
                        var project = DbContext.Projects.FirstOrDefault(x => x.Id == tempGuid);
                        if (project != null)
                        {
                            viewModel.Add(new ProjectViewModel(project));
                        }
                    }
                }

                Clients.Caller.OnGetProjects(viewModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SignalR Error: " + ex.Message);
                //TODO: Add Serilog
            }
        }
    }
}