using System.IO;
using WS.Games.Elo.Lib.Utilities;

namespace WS.Games.Elo.Lib.Repositories
{
    public class JsonRepositoryFactory : IRepositoryFactory
    {
        private readonly string baseDirectory;

        public JsonRepositoryFactory(string baseDirectory)
        {
            this.baseDirectory = baseDirectory;
        }

        public IRepository<TModel> GetRepository<TModel>() where TModel : class, IIdentifiable<TModel>
        {
            var fileName = Path.Combine(baseDirectory, Path.ChangeExtension(typeof(TModel).Name, "json"));
            var fileWrapper = new FileWrapper(fileName);
            return new JsonRepository<TModel>(fileWrapper);
        }
    }
}