using WS.Games.Elo.Lib.Services;

namespace WS.Games.Elo.Web
{
    internal class PlayerServiceConfiguration : IPlayerServiceConfiguration
    {
        public int NewPlayerRating => 1000;
    }
}