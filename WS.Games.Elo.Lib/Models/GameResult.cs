using System;
using System.Collections.Generic;
using WS.Games.Elo.Lib.Utilities;

namespace WS.Games.Elo.Lib.Models
{
    public class GameResult : IIdentifiable<GameResult>
    {
        public string Game { get; set;}

        public string Location { get; set;}

        public DateTime StartTime { get; set;}

        public List<List<PlayerResult>> PlayerResultsByPosition { get; set; }

        public bool IdentifiesWith(GameResult other)
        {
            return Game == other.Game && StartTime == other.StartTime;
        }
    }
}