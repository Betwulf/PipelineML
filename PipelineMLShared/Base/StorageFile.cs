using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class StorageFile : IStorage
    {
        private string _rootDir = "C:\\Temp\\";

        

        public IQueryable<Tuple<string, string>> GetAll(string path)
        {
            var listOfFilenames = GetAllFilenames(path);
            List<Tuple<string, string>> listOfData = new List<Tuple<string, string>>();
            foreach (var item in listOfFilenames)
            {
                listOfData.Add(new Tuple<string, string>(item, ReadData(path, item)));
            }
            return listOfData.AsQueryable();
        }


        public IQueryable<string> GetAllFilenames(string path)
        {
            List<string> listOfFilenames = new List<string>();
            var dir = new DirectoryInfo(FullDirectory(path));
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (var item in fileList)
            {
                listOfFilenames.Add(item.Name);
            }
            return listOfFilenames.AsQueryable();
        }


        public string ReadData(string path, string file)
        {
            Directory.CreateDirectory(FullDirectory(path));
            string pathString = FullPath(path, file);
            if (File.Exists(pathString))
            {
                return File.ReadAllText(pathString);
            }
            return null;
        }


        public void RemoveData(string path, string file)
        {
            string pathString = FullPath(path, file);
            if (File.Exists(pathString))
            {
                File.Delete(pathString);
            }
        }


        public void WriteData(string path, string file, string data)
        {
            string dir = FullDirectory(path);
            Directory.CreateDirectory(dir);
            File.WriteAllText(FullPath(path, file), data);
        }

        public void EmptyPath(string path)
        {
            var dir = new DirectoryInfo(FullDirectory(path));
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.json", SearchOption.AllDirectories);
            foreach (var item in fileList)
            {
                File.Delete(item.FullName);
            }
        }


        private string FullDirectory(string path)
        {
            return Path.Combine(_rootDir, path);
        }


        private string FullPath(string path, string file)
        {
            return Path.Combine(_rootDir, path, file);
        }
    }
}
