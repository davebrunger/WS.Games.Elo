using WS.Games.Elo.Lib.Utilities;

namespace WS.Games.Elo.Lib.Repositories
{
    public interface IRepositoryFactory
    {
         IRepository<TModel> GetRepository<TModel>() where TModel : class, IIdentifiable<TModel>;
    }
}