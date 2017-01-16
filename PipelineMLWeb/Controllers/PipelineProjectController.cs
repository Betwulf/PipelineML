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
        public async Task<ActionResult> Edit()
        {
            var guidstr = Guid.Parse("00000000-0000-0000-0000-000000000000").ToString();
            if (User.Identity.Name == "a@a.com")
            {
                //var user = UserManager.FindById(User.Identity.GetUserId());
                ApplicationUser user = UserManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                Debug.WriteLine("Found user a@a");
                if (user.Claims.FirstOrDefault(x => x.ClaimValue == guidstr) == null)
                {
                    Debug.WriteLine("User a@a doesnt have claim --- trying to add");
                    var aClaim = new Claim(PipelineClaimsTypes.PipelineProject, guidstr);
                    await UserManager.AddClaimAsync(user.Id, aClaim);
                    await UserManager.UpdateAsync(user);
                    ((ClaimsIdentity)User.Identity).AddClaim(aClaim);
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