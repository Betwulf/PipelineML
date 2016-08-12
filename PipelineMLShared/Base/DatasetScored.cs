using System;
using System.Data;

namespace PipelineMLCore
{
    public class DatasetScored : DatasetBase, IDataset
    {
        public DatasetScored(IDatasetDescriptor descriptor) : base(descriptor)
        {
            if (!Descriptor.ColumnDescriptions.Exists( x => x.IsScore == true) || !Descriptor.ColumnDescriptions.Exists(x => x.IsScoreProbability == true))
            {
                throw new Exception($"Cannot find any scored columns in the scored Dataset");
            }
        }

    }
}
