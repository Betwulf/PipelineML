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
        public void GetAvailableClassTypes(string interfaceType)
        {
            List<Type> TypeList;
            TypeList = SearchClasses.SearchForClassesThatImplementInterface(Type.GetType(interfaceType));

            Clients.Caller.OnGetAvailableClassTypes(TypeList);

        }
    }
}