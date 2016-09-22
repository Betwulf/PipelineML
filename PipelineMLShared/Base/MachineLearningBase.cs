using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PipelineMLCore
{
    public abstract class MachineLearningBase 
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        public string Name { get { return Config.Name; } }





        /// <summary>
        /// Returns the differences between the output and expected output
        /// </summary>
        /// <param name="output"></param>
        /// <param name="desiredOutput"></param>
        /// <returns>the squared error</returns>
        protected double[] GetErrorVector(double[] output, double[] desiredOutput)
        {

            if (output.Length != desiredOutput.Length)
            {
                throw new IndexOutOfRangeException($"Output Length: {output.Length} does not match desired output length: {desiredOutput.Length}");
            }
            double[] errorVector = new double[output.Length];
            for (int i = 0; i < output.Length; i++)
            {
                errorVector[i] = output[i] - desiredOutput[i];
            }
            return errorVector;
        }




        /// <summary>
        /// Sort through the data that was given, determine if it has a label, and a training flag, and features
        /// Gather information on the values in each feature column, min-max range, is it continuous or discrete.
        /// Create a map of actual data to normalized data.
        /// ignore columns that are not features.
        /// </summary>
        /// <param name="datasetIn"></param>
        /// <returns>prepped ML dataset</returns>
        protected DatasetML PrepareDataset(DatasetBase datasetIn, Action<string> updateMessage, IMachineLearningProcess self)
        {
            if (datasetIn.Descriptor.ColumnDescriptions.Find(x => x.IsTraining == true) == null)
            { throw new PipelineException("Cannot find Training rows in dataset. Please set training data before machine learning", datasetIn, self, updateMessage); }

            // preprocess columns
            DatasetML mlData = new DatasetML(new DatasetDescriptorML() { ColumnDescriptions = new List<DataColumnML>(), Name = datasetIn.Name });
            mlData.NumberOfFeatures = 0;
            mlData.NumberOfLabels = 0;
            mlData.Table = datasetIn.Table;
            foreach (var col in datasetIn.Descriptor.ColumnDescriptions)
            {
                var newCol = new DataColumnML(col);
                mlData.Descriptor.ColumnDescriptions.Add(newCol);

                if (newCol.IsFeature || newCol.IsLabel)
                {
                    newCol.ColumnSet = (from row in datasetIn.Table.AsEnumerable()
                                        select row[col.Name]).Distinct();
                    newCol.MinRange = 0.0;
                    if ((col.DataType != typeof(double)) || (col.IsCategory))
                    {
                        newCol.MaxRange = newCol.ColumnSet.Count() - 1;
                    }
                    else
                    {
                        // the column is a double and max range is what it is, no mapping needed
                        // the double multiplier is to give the tree room for unexpected higher double values in non-training data
                        newCol.MaxRange = ((double)newCol.ColumnSet.Max()) * 2.0;
                    }

                    // generate a map of existing values to numerical categories
                    newCol.ColumnMap = new Dictionary<object, int>();
                    for (int i = 0; i < newCol.ColumnSet.Count(); i++)
                    {
                        newCol.ColumnMap.Add(newCol.ColumnSet.ElementAt(i), i);
                    }
                    if (col.IsLabel)
                    {
                        mlData.NumberOfLabels++;
                        if (col.IsFeature)
                            throw new PipelineException($"Column {col.Name} cannot be both a feature and a label column", datasetIn, self, updateMessage);
                        if (newCol.MaxRange > 10)
                            throw new PipelineException($"Trying to predict {newCol.MaxRange} number of possible outcomes is too many. Reduce the distinct values of your label column {col.Name}.", datasetIn, self, updateMessage);
                    }
                    else if (col.IsFeature)
                    { mlData.NumberOfFeatures++; }
                }
            }
            if (mlData.NumberOfFeatures == 0)
                throw new PipelineException($"There are no columns designated as a feature. Please add a preprocessor to have at least one feature to input into the ML algorithm", datasetIn, self, updateMessage);

            // Add a column to store the ml input data for the entire row
            var MLInputDataColumn = new DataColumnML() { Name = nameof(DataColumnML.IsMLInputData), Description = nameof(DataColumnML.IsMLInputData), DataType = typeof(double[]), IsMLInputData = true };
            var MLLabelDataColumn = new DataColumnML() { Name = nameof(DataColumnML.IsMLLabelData), Description = nameof(DataColumnML.IsMLLabelData), DataType = typeof(double[]), IsMLLabelData = true };
            mlData.Descriptor.ColumnDescriptions.Add(MLInputDataColumn);
            mlData.Descriptor.ColumnDescriptions.Add(MLLabelDataColumn);
            datasetIn.Table.Columns.Add(MLInputDataColumn.Name, MLInputDataColumn.DataType);
            datasetIn.Table.Columns.Add(MLLabelDataColumn.Name, MLLabelDataColumn.DataType);
            string mlColInputName = nameof(DataColumnML.IsMLInputData);
            string mlColLabelName = nameof(DataColumnML.IsMLLabelData);

            // setup training data rows
            var trainingRows = mlData.Table.Select($"{nameof(DataColumnBase.IsTraining)} = true");
            int trainingRowCount = trainingRows.Count();
            mlData.labels = new double[trainingRowCount][];
            mlData.inputs = new double[trainingRowCount][];

            // populate the ml column (type double[]) for each row, with each column's interpreted data for input
            // filter only the columns that are features or labels to be sent to the ml algorithm
            int rowNum = 0;
            foreach (DataRow row in datasetIn.Table.Rows)
            {
                row[MLInputDataColumn.Name] = new double[mlData.NumberOfFeatures];
                row[MLLabelDataColumn.Name] = new double[mlData.NumberOfLabels];
                int featureColNum = 0;
                int labelColNum = 0;
                foreach (var col in mlData.Descriptor.ColumnDescriptions)
                {
                    // check if we have to categorize the input for ml
                    if (col.IsLabel || !col.IsFeature)
                    {
                        // Don't increment colnum, only features are input columns
                        if (col.IsLabel)
                        {
                            ((double[])row[MLLabelDataColumn.Name])[labelColNum] = col.ColumnMap[row[col.Name]];
                            labelColNum++;
                        }
                    }
                    else if ((col.DataType != typeof(double)) || (col.IsCategory))
                    {
                        // if it isn't already a double, convert it
                        ((double[])row[MLInputDataColumn.Name])[featureColNum] = col.ColumnMap[row[col.Name]];
                        featureColNum++;
                    }
                    else
                    {
                        // Then the data is a double already and is not indicated to be a category, so take it in as is.
                        ((double[])row[MLInputDataColumn.Name])[featureColNum] = (double)row[col.Name];
                        featureColNum++;
                    }
                }
                rowNum++;
            }
            mlData.Table = datasetIn.Table;


            // find the column that has the generated ml input data
            for (int i = 0; i < trainingRowCount; i++)
            {
                // Map inputs to ml data
                mlData.labels[i] = (double[])trainingRows[i][mlColLabelName];
                mlData.inputs[i] = (double[])trainingRows[i][mlColInputName];
            }

            return mlData;
        }


    }
}
