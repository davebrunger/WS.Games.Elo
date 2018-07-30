using WS.Games.Elo.Lib.Elo;
using WS.Games.Elo.Lib.Utilities;

namespace WS.Games.Elo.Lib.Models
{
    public class Player : IIdentifiable<Player>, IImmutableRatingHolder<Player>
    {
        public string Name {get; set;}

        public int Rating {get;set;}

        public bool IdentifiesWith(Player other)
        {
            return Name == other.Name;
        }

        public Player UpdateRating(int newRating) => new Player{
            Name = Name,
            Rating = newRating
        };

        bool IIdentifiable<IImmutableRatingHolder<Player>>.IdentifiesWith(IImmutableRatingHolder<Player> other)
        {
            return IdentifiesWith(other.UpdateRating(other.Rating));
        }
    }
}