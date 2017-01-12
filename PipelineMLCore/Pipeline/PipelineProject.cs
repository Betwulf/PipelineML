using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class PipelineProject
    {
        private IKernel _kernel;

        public PipelineProject()
        {
            Id = Guid.NewGuid();
        }

        public void Configure(IKernel kernel)
        {
            _kernel = kernel;
        }

        [Required, MaxLength(40)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid PipelineDefinitionGuid { get; set; }

        public List<PipelineResultsId> RunResults { get; set; }
    }
}
