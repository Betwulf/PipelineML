using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Configuration;

namespace PipelineMLCore
{
    public class JsonRepository<T> where T : INamed
    {
        private readonly string mRootDir;
        private Dictionary<string, T> DocStore;
        private readonly bool isCached;

        public JsonRepository(string aRootDir)
        {
            DocStore = new Dictionary<string, T>();
            mRootDir = aRootDir;
            isCached = true;
        }

        public string CollectionId
        { get { return this.GetType().GenericTypeArguments[0].FullName; } }

        public string DatabaseLink
        { get { return this.GetType().ToString(); } }

        public string SelfLink
        {
            // Should Remove and delete from interface too
            get { return this.GetType().ToString(); }
        }


        public string GetDir { get { return System.IO.Path.Combine(mRootDir, CollectionId); } }

        private string GetFilename(string name)
        {
            return System.IO.Path.Combine(GetDir, name + ".json");
        }

        public async Task<T> CreateAsync(T entity)
        {
            await Task.Run(() =>
            {

                string pathString = GetDir;
                Directory.CreateDirectory(pathString); // Create Directory if it isn't already made
                pathString = GetFilename(entity.Name);
                if (!File.Exists(pathString))
                {
                    Console.WriteLine("Saving New File \"{0}\" .", pathString);
                    File.WriteAllText(pathString, JsonConvert.SerializeObject(entity));
                    if (isCached) DocStore.Add(entity.Name, entity);
                }
                else
                {
                    Console.WriteLine("Overwriting File \"{0}\" .", pathString);
                    File.WriteAllText(pathString, JsonConvert.SerializeObject(entity));
                    if (isCached)
                    {
                        DocStore.Remove(entity.Name); // Overwrite cache
                        DocStore.Add(entity.Name, entity);
                    }
                }
            });
            return entity;

        }

        public async Task DeleteAsync(string id)
        {
            await Task.Run(() =>
            {
                if (isCached) DocStore.Remove(id);
                string pathString = GetFilename(id);
                Console.WriteLine("Deleting file: {0}\n", pathString);
                if (File.Exists(pathString))
                {
                    File.Delete(pathString);
                }
                else
                {
                    Console.WriteLine("File \"{0}\" did not exist.", pathString);
                }
            });
        }

        public IEnumerable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            var dir = new DirectoryInfo(GetDir);
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.json", SearchOption.AllDirectories);
            List<T> tempList = new List<T>();
            foreach (var item in fileList)
            {
                T storedDoc = default(T);
                T tempObject = default(T);
                var id = item.Name.Remove(item.Name.Length - item.Extension.Length);
                if (isCached && DocStore.TryGetValue(id, out storedDoc))
                {
                    tempObject = storedDoc;
                }
                else
                {
                    tempObject = JsonConvert.DeserializeObject<T>(File.ReadAllText(item.FullName));
                    if (isCached) DocStore.Add(id, tempObject);
                }
                tempList.Add(tempObject);
            }
            return new EnumerableQuery<T>(tempList);
        }



        public T GetById(string id)
        {
            T storedDoc;
            if (isCached && DocStore.TryGetValue(id, out storedDoc))
            {
                //Console.WriteLine("returning stored doc: " + id);
                return storedDoc;
            }
            string pathString = GetDir;
            Directory.CreateDirectory(pathString); // Create Directory if it isn't already made
            pathString = GetFilename(id);
            if (File.Exists(pathString))
            {
                T theDoc = JsonConvert.DeserializeObject<T>(File.ReadAllText(pathString));
                if (isCached) DocStore.Add(id, theDoc);
                return theDoc;
            }
            else
            {
                return default(T);
            }
        }

        public async Task<T> UpdateAsync(string id, T entity)
        {
            return await CreateAsync(entity);
        }

        public void DeleteDatabase()
        {
            var dir = new DirectoryInfo(GetDir);
            IEnumerable<FileInfo> fileList = dir.GetFiles("*.json", SearchOption.AllDirectories);
            List<T> tempList = new List<T>();
            foreach (var item in fileList)
            {
                File.Delete(item.FullName);
            }
        }
    }
}
