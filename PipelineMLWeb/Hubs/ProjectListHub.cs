using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace PipelineMLWeb.Hubs
{
    public class ProjectListHub : Hub
    {
        public void GetProjects()
        {
            var identity = (ClaimsIdentity)Context.User.Identity;
            var projects = identity.Claims.Any( x => x.Type == PipelineClaimsTypes.PipelineProject);
            // TODO: return projects to client 

        }
    }
}