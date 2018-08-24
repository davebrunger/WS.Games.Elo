using WS.Games.Elo.Lib.Utilities;

namespace WS.Games.Elo.Lib.Models
{
    public class User : IIdentifiable<User>
    {
        public string Name {get; set;}

        // TODO Salt and Hash Passwords
        public string Password {get; set;}

        public bool IdentifiesWith(User other)
        {
            return Name == other.Name;
        }
    }
}