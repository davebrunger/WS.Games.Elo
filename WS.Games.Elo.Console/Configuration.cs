using WS.Games.Elo.Lib.Services;

namespace WS.Games.Elo.Console
{
    internal class Configuration : IPlayerServiceConfiguration, IGameServiceConfiguration
    {
        public int NewPlayerRating => 1000;
    }
}