using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PipelineMLWeb.Models
{
    public class CreatePipelinePartViewModel
    {
        public string projectId { get; set; }

        public string classType { get; set; }

        public int columnNumber { get; set; }

    }
}