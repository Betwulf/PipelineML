using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace PipelineMLCore
{
    public static class ExtendData
    {
        public static string ToExpandedString(this double[] x)
        {
            if (x.Length == 0) return "'{}";
            string retval = "{ ";
            for (int i = 0; i < x.Length; i++)
            {
                if (i == x.Length - 1)
                    retval += x[i].ToString();
                else
                    retval += x[i].ToString() + " | ";
            }
            retval += " }";
            return retval;
        }
        public static void WriteToCsvFile(this DataTable dataTable, string filePath)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dataTable.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                     string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
