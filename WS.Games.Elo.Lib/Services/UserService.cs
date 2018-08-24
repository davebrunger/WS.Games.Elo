using WS.Games.Elo.Lib.Models;
using WS.Games.Elo.Lib.Repositories;

namespace WS.Games.Elo.Lib.Services
{
    public class UserService
    {
        private readonly IRepositoryFactory repositoryFactory;

        public UserService(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public User GetUser(string name)
        {
            using (var userRepository = repositoryFactory.GetRepository<User>())
            {
                return userRepository.Get(new User { Name = name });
            }
        }
    }
}