using WS.Games.Elo.Lib.Services;

namespace WS.Games.Elo.Web
{
    internal class Configuration : IPlayerServiceConfiguration, IGameServiceConfiguration
    {
        public int NewPlayerRating => 1000;
    }
}