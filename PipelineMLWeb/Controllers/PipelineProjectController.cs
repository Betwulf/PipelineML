using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using PipelineMLWeb.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using PipelineMLCore;
using PipelineMLWeb.DataContexts;
using System.Web.Routing;

namespace PipelineMLWeb.Controllers
{
    [Authorize]
    public class PipelineProjectController : Controller
    {
        private ApplicationUserManager _userManager;

        public PipelineProjectController()
        {

        }
        public PipelineProjectController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectViewModel model)
        {
            var newProj = new PipelineProject();
            newProj.Name = model.Name;
            newProj.Description = model.Description;
            var newPipelineDef = new PipelineDefinition();
            newPipelineDef.Configure(Startup.CreateNinject());
            newProj.PipelineDefinitionGuid = newPipelineDef.Id;
            newPipelineDef.Name = model.Name;

       
            var db = HttpContext.GetOwinContext().Get<PipelineDbContext>();
            // Save definition file
            db.SavePipelineDefinition(newPipelineDef.Id, newPipelineDef);
            // Save project
            db.Projects.Add(newProj);
            int x = await db.SaveChangesAsync();
            if (x == 1) // saved one change
            {
                ApplicationUser user = UserManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var aClaim = new Claim(PipelineClaimsTypes.PipelineProject, newProj.Id.ToString());
                await UserManager.AddClaimAsync(user.Id, aClaim);
                await UserManager.UpdateAsync(user);
                ((ClaimsIdentity)User.Identity).AddClaim(aClaim);

                return RedirectToAction("Edit", new RouteValueDictionary( new { controller = "PipelineProject", action = "Edit", id = newProj.Id.ToString() }));
            }
            // TODO: Handle Errors
            return View();
        }

        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.projectId = id;
            ApplicationUser user = UserManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Debug.WriteLine($"Edit: user {user.UserName} Guid: {id}");
            if (user.Claims.FirstOrDefault(x => x.ClaimType == PipelineClaimsTypes.PipelineProject && x.ClaimValue == id) == null)
            {
                Debug.WriteLine($"User {user.UserName} doesnt have claim --- how did they get here?");
                return RedirectToAction("Error", new RouteValueDictionary(new { controller = "Home", action = "Error",  id = "You do not have the right to this project." } ));
            }
            else
            {
                Debug.WriteLine($"User {user.UserName} has CLAIM!");
            }
            return View();
        }


        public ActionResult Schema()
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            return null;
            //return Content(JSchema.)
        }
    }
}