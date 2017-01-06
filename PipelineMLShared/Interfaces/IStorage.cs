using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public interface IStorage
    {
        string ReadData(string path, string filename);

        void WriteData(string path, string filename, string data);

        void RemoveData(string path, string filename);

        /// <summary>
        /// Retuns filenames and data for all found in path
        /// </summary>
        /// <param name="path">Path to search for all files</param>
        /// <returns>A list of tuples containing filename, data</returns>
        IQueryable<Tuple<string, string>> GetAll(string path);

        IQueryable<string> GetAllFilenames(string path);

        void EmptyPath(string path);
    }
}
