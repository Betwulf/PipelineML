using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore.Pipeline
{
    public class PipelineProject
    {
        private IKernel _kernel;

        public PipelineProject()
        {

        }

        public void Configure(IKernel kernel)
        {
            _kernel = kernel;
        }

        [Required, MaxLength(40)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid Id { get; set; }

        public PipelineDefinition Definition { get; set; }

        public List<PipelineResults> Results { get; set; }
    }
}
