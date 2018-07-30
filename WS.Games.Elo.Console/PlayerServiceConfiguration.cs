using WS.Games.Elo.Lib.Services;

namespace WS.Games.Elo.Console
{
    internal class PlayerServiceConfiguration : IPlayerServiceConfiguration
    {
        public int NewPlayerRating => 1000;
    }
}