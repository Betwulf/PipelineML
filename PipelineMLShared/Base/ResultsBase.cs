using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class ResultsBase
    {

        public StringBuilder Log { get; set; }


        public Action<string> GetLoggedUpdateMessage(Action<string> updateMessage)
        {
            Action<string> updateAll = x => { updateMessage(x); Log.Append(x); };
            return updateAll;
        }

        protected string GetColumnName(DataColumnBase col)
        {
            return $"{col.Name}({col.DataType})";
        }

        protected string GetColumnDetails(DataColumnBase col)
        {
            string outString = $"Column: {col.Name} \t Type: {col.DataType.Name}";
            if (col.IsCategory)
                outString += "\t IsCategory";
            if (col.IsFeature)
                outString += "\t IsFeature";
            if (col.IsLabel)
                outString += "\t IsLabel";
            if (col.IsScore)
                outString += "\t IsScore";
            if (col.IsScoreProbability)
                outString += "\t IsScoreProbability";
            if (col.IsTraining)
                outString += "\t IsTraining";
            return outString;
        }


    }
}
