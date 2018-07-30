using System.Collections.Generic;
using WS.Games.Elo.Lib.Utilities;

namespace WS.Games.Elo.Lib.Models
{
    public class Game : IIdentifiable<Game>
    {
        public string Name {get; set;}        

        public string GameType {get; set;}        

        public int MinimumPlayerCount {get; set;}

        public int MaximumPlayerCount {get; set;}

        public string GetIdentifier() => Name;

        public bool IdentifiesWith(Game other)
        {
            return Name == other.Name;
        }
    }
}