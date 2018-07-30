using System;
using System.Linq;
using WS.Games.Elo.Lib.Utilities;

namespace WS.Games.Elo.Lib.Repositories
{
    public interface IRepository<TModel> : IDisposable where TModel : class, IIdentifiable<TModel>
    {
        IQueryable<TModel> Get();

        TModel Get(TModel key);

        bool Put(TModel item);

        bool Delete(TModel key);

        void Clear();
    }
}