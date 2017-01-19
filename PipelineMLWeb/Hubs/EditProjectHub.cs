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