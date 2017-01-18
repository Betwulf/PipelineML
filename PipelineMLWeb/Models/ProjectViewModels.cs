using PipelineMLCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PipelineMLWeb.Models
{
    // Defines the shell of the pieces of a pipeline
    // to be used to get the details of the part when necessary
    public class PipelinePartViewModel
    {
        public Guid Id { get; set; }

        [Required, MaxLength(40)]
        public string Name { get; set; }

        public string ClassName { get; set; }
    } 

    public class ProjectViewModel
    {
        public ProjectViewModel()
        {
            Parts = new List<PipelinePartViewModel>();
        }

        public ProjectViewModel(PipelineProject proj)
        {
            Parts = new List<PipelinePartViewModel>();
            Name = proj.Name;
            Id = proj.Id;
            Description = proj.Description;
            // TODO: populate parts
        }

        public Guid Id { get; set; }

        [Required, MaxLength(40)]
        public string Name { get; set; }

        
        public string Description { get; set; }

        public List<PipelinePartViewModel> Parts { get; set; }

    }
}