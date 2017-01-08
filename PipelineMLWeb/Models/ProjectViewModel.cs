using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PipelineMLWeb.Models
{
    public class PipelinePartViewModel
    {
        public Guid Id { get; set; }

        [Required, MaxLength(40)]
        public string Name { get; set; }

        public string InterfaceName { get; set; }

    } 

    public class ProjectViewModel
    {
        public ProjectViewModel()
        {
            Parts = new List<PipelinePartViewModel>();
        }

        public Guid Id { get; set; }

        [Required, MaxLength(40)]
        public string Name { get; set; }

        
        public string Description { get; set; }

        public List<PipelinePartViewModel> Parts { get; set; }

    }
}