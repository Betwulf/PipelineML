using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;

namespace PipelineMLWeb.Controllers
{
    public class PipelineProjectController : Controller
    {
        // GET: PipelineProject
        public ActionResult Edit()
        {
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