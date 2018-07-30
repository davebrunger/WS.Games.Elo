using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WS.Games.Elo.Lib.Utilities;

namespace WS.Games.Elo.Lib.Repositories
{
    public class JsonRepository<TModel> : IRepository<TModel>
        where TModel : class, IIdentifiable<TModel>
    {
        private readonly IFileWrapper fileWrapper;

        private List<TModel> _data;

        public string FileName => fileWrapper.FileName;

        public JsonRepository(IFileWrapper fileWrapper)
        {
            this.fileWrapper = fileWrapper;
        }

        public IQueryable<TModel> Get()
        {
            EnsureDataLoaded();
            return Copy(_data).AsQueryable();
        }

        public TModel Get(TModel key)
        {
            EnsureDataLoaded();
            var result = _data.SingleOrDefault(i => i.IdentifiesWith(key));
            return result == null ? null : Copy(result);
        }

        public bool Put(TModel item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var result = Delete(item, false);
            _data.Add(Copy(item));
            SaveData();
            return result;
        }

        public bool Delete(TModel key)
        {
            return Delete(key, true);
        }

        private bool Delete(TModel key, bool saveData)
        {
            EnsureDataLoaded();
            var oldItem = _data.SingleOrDefault(i => i.IdentifiesWith(key));
            if (oldItem == null)
            {
                return false;
            }
            _data.Remove(oldItem);
            if (saveData)
            {
                SaveData();
            }
            return true;
        }

        public void Clear()
        {
            _data = new List<TModel>();
            SaveData();
        }

        private void SaveData()
        {
            if (_data != null)
            {
                var data = JsonConvert.SerializeObject(_data, Formatting.Indented);
                fileWrapper.WriteAllText(data);
            }
        }

        private void EnsureDataLoaded()
        {
            if (_data == null)
            {
                if (fileWrapper.FileExists)
                {
                    var data = fileWrapper.ReadAllText();
                    _data = JsonConvert.DeserializeObject<IEnumerable<TModel>>(data).ToList();
                }
                else
                {
                    _data = new List<TModel>();
                }
            }
        }

        private static T Copy<T>(T obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        public void Dispose()
        {
            // No Resources to dispose of
        }
    }
}