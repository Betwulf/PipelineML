using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using PipelineMLWeb.DataContexts;
using PipelineMLWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PipelineMLWeb.Hubs
{
    public static class HubExtensions
    {
        public static PipelineDbContext GetPipelineDbContext(this Hub me)
        {
            var kernel = Startup.CreateNinject();
            var DbContext = new PipelineDbContext(kernel);
            return DbContext;
        }


        public static ApplicationUser GetApplicationUser(this Hub me)
        {
            var dbb = new ApplicationDbContext();
            ApplicationUser currentUser = dbb.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            return currentUser;
        }
    }
}