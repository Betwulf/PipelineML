using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public interface ISearchableClass
    {
        string FriendlyName { get; }
        string Description { get; }

    }
}
