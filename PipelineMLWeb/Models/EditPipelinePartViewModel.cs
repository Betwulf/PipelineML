using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PipelineMLWeb.Models
{
    public class EditPipelinePartViewModel
    {
        public string projectId { get; set; }

        public string pipelinePartId { get; set; }

        public int columnNumber { get; set; }

        public string classType { get; set; }

    }
}