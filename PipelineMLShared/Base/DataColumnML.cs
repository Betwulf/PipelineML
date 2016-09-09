using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DataColumnML : DataColumnBase
    {
        // The minimum value of ml converted data
        public double MinRange { get; set; }

        // The maximum value of ml converted data
        public double MaxRange { get; set; }

        // The set of all possible values in this column
        public IEnumerable<object> ColumnSet { get; set; }

        // the map of raw data values to ML values (This may not work for all ML, only DecisionTrees known so far, and not needed for some columns like non category columns that are double, or label columns, or non feature columns)
        public Dictionary<object, int> ColumnMap { get; set; }

        // is this column containing the converted ml input data for the whole row
        public bool IsMLInputData { get; set; }

        public bool IsMLLabelData { get; set; }



        public DataColumnML()
        {

        }

        public DataColumnML(DataColumnBase source)
        {
            Name = source.Name;
            Description = source.Description;
            DataType = source.DataType;
            Id = source.Id;
            IsCategory = source.IsCategory;
            IsFeature = source.IsFeature;
            IsLabel = source.IsLabel;
            IsScore = source.IsScore;
            IsScoreProbability = source.IsScoreProbability;
            IsTraining = source.IsTraining;
        }

    }
}
