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

namespace PipelineMLWeb.Controllers
{
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


        // GET: PipelineProject
        public ActionResult Edit()
        {
            if (User.Identity.Name == "a@a.com")
            {
                Debug.WriteLine("Found user a@a");
                if (!((System.Security.Claims.ClaimsIdentity)User.Identity).HasClaim(PipelineClaimsTypes.PipelineProject, Guid.Parse("00000000-0000-0000-0000-000000000000").ToString()))
                {
                    Debug.WriteLine("User a@a doesnt have claim --- trying to add");
                    var aClaim = new Claim(PipelineClaimsTypes.PipelineProject, Guid.Parse("00000000-0000-0000-0000-000000000000").ToString());
                    UserManager.AddClaimAsync(HttpContext.User.Identity.Name, aClaim);
                    //var user = UserManager.FindById(User.Identity.GetUserId());
                    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    UserManager.UpdateAsync(user);
                    //((System.Security.Claims.ClaimsIdentity)User.Identity).AddClaim(aClaim);
                    //UserManager.UpdateAsync(User.Identity.GetApplicationUser());
                }
                else
                {
                    Debug.WriteLine("User a@a has CLAIM!");
                }
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