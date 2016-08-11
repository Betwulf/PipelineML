using Microsoft.CSharp;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace PipelineMLCore
{
    public class DataTransformAddColumn : IDataTransform, ISearchableClass
    {
        public string Name { get; set; }

        public string FriendlyName { get { return "Add Column Data Transform"; } }

        public string Description { get { return "Run custom C# code to generate a new column of data"; } }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DataTransformConfigAddColumn ConfigInternal { get { return Config as DataTransformConfigAddColumn; } }

        public string CodeHeader
        {
            get
            {
                return @"
            using System;
            using System.Collections.Generic;
            using System.ComponentModel;
            using System.Linq;
            using System.Data;

            namespace PipelineMLCore 
            {
                public class TransformDynamic
                {
                    public DataTable Transform(IDataset datasetIn)
                    {";
            }
        }

        public string CodeFooter
        {
            get
            { return @"
                    }
                }
            }"; }
        }
        public string CodeStarter => @"var tableOut = new DataTable(); 
            tableOut.Columns.Add(" + "\"" + ConfigInternal?.NewColumn.Name + "\", typeof(" + ConfigInternal.NewColumn.DataType.Name + @"));
            for (int i = 0; i < datasetIn.Table.Rows.Count; i++)
            {
                tableOut.Rows.Add(new object[] { 0 });
            }
            return tableOut;";
        


        public static List<string> ReferencedDlls = new List<string>
            {
                "mscorlib.dll",
                "System.dll",
                "System.Core.dll",
                "System.Xml.dll",
                "PipelineMLShared.dll",
                "System.Linq.dll",
                "System.ComponentModel.dll",
                "System.Data.dll"
            };


        public DataTransformAddColumn()
        {
            Config = new DataTransformConfigAddColumn();
        }

        public void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DataTransformConfigAddColumn>(jsonConfig);
            Name = Config.Name;
        }



        public IDataset Transform(IDataset datasetIn)
        {
            // Compile and run Dynamic Code
            // TODO: filter code for bad behavior
            string totalCode = CodeHeader + ConfigInternal.Code + CodeFooter;
            Dictionary<string, string> providerOptions = new Dictionary<string, string>
                {
                    {"CompilerVersion", "v4.0"}
                };
            CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

            CompilerParameters compilerParams = new CompilerParameters(assemblyNames: ReferencedDlls.ToArray())
            {
                WarningLevel = 0,
                TreatWarningsAsErrors = false,
                GenerateInMemory = true,
                GenerateExecutable = false
            };

            CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, totalCode);

            if (results.Errors.Count != 0)
            {
                string errorString = $"Errors: {results.Errors.Count} ";
                foreach (var error in results.Errors)
                {
                    errorString += @"
Error: " + error.ToString();
                }
                throw new Exception(errorString);
            }
                

            object o = results.CompiledAssembly.CreateInstance("PipelineMLCore.TransformDynamic");
            MethodInfo mi = o.GetType().GetMethod("Transform");
            DataTable dataTableOut = mi.Invoke(o, new object[1] { datasetIn }) as DataTable;


            // Merge new column into Dataset
            datasetIn.Descriptor.ColumnNames.Add(ConfigInternal.NewColumn);
            datasetIn.Table.Columns.Add(ConfigInternal.NewColumn.Name, ConfigInternal.NewColumn.DataType);
            // copy over values
            for (int i = 0; i < datasetIn.Table.Rows.Count; i++)
            {
                var row = datasetIn.Table.Rows[i];
                row[ConfigInternal.NewColumn.Name] = dataTableOut.Rows[i][ConfigInternal.NewColumn.Name];
            }
            return datasetIn;
        }
    }
}
