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
        private static string jsonExt = ".json";
        private Dictionary<string, T> DocCache;
        private readonly bool isCached;
        private IStorage _storage;
        private string _path;

        public JsonRepository(string path, IStorage storage)
        {
            DocCache = new Dictionary<string, T>();
            isCached = true;
            _storage = storage;
            _path = Path.Combine(path, GetType().GenericTypeArguments[0].FullName);
        }


        private string GetFilename(string name)
        {
            return name + jsonExt;
        }

        public async Task<T> CreateAsync(T entity, Action<string> updateMessage)
        {
            await Task.Run(() =>
            {
                _storage.WriteData(_path, GetFilename(entity.Name), JsonConvert.SerializeObject(entity));
                updateMessage($"Saving New File \"{_path}\\{GetFilename(entity.Name)}\" .");
                if (isCached)
                {
                    if (DocCache.Any(x => x.Key == entity.Name))
                        DocCache.Remove(entity.Name); // Overwrite cache with updated object
                    DocCache.Add(entity.Name, entity);
                }
            });
            return entity;

        }

        public async Task DeleteAsync(string entityName)
        {
            await Task.Run(() =>
            {
                if (isCached) DocCache.Remove(entityName);
                _storage.RemoveData(_path, GetFilename(entityName));
                Console.WriteLine($"Deleting file: \"{_path}\\{GetFilename(entityName)}\" ");
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
            var filenameList = _storage.GetAllFilenames(_path);
            List<T> tempList = new List<T>();
            foreach (var name in filenameList)
            {
                T cachedDoc = default(T);
                T tempObject = default(T);
                var id = name.Remove(name.Length - jsonExt.Length);
                if (isCached && DocCache.TryGetValue(id, out cachedDoc))
                {
                    tempObject = cachedDoc;
                }
                else
                {
                    tempObject = JsonConvert.DeserializeObject<T>(_storage.ReadData(_path, name));
                    if (isCached) DocCache.Add(id, tempObject);
                }
                tempList.Add(tempObject);
            }
            return new EnumerableQuery<T>(tempList);
        }



        public T GetById(string id)
        {
            T storedDoc;
            if (isCached && DocCache.TryGetValue(id, out storedDoc))
            {
                //Console.WriteLine("returning stored doc: " + id);
                return storedDoc;
            }
            string data = _storage.ReadData(_path, GetFilename(id));
            if (data != null)
            {
                T theDoc = JsonConvert.DeserializeObject<T>(data);
                if (isCached) DocCache.Add(id, theDoc);
                return theDoc;
            }
            return default(T);
        }

        public async Task<T> UpdateAsync(string id, T entity, Action<string> updateMessage)
        {
            return await CreateAsync(entity, updateMessage);
        }

        public void DeleteDatabase()
        {
            DocCache.Clear();
            _storage.EmptyPath(_path);
        }
    }
}
