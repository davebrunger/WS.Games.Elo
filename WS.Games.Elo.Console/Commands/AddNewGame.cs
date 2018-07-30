using System;
using System.Collections.Generic;
using NDesk.Options;
using WS.Games.Elo.Lib.Services;
using WS.Utilities.Console;

namespace WS.Games.Elo.Console.Commands
{
    public class AddNewGame : ICommand
    {
        private readonly IOutputWriter outputWriter;
        private readonly GameService gameService;

        public string Name => "add-new-game";

        public AddNewGame(IOutputWriter outputWriter, GameService gameService)
        {
            this.outputWriter = outputWriter;
            this.gameService = gameService;
        }

        public Action GetAction(IEnumerable<string> options)
        {
            string name = null;
            string gameType = null;
            int? minimumPlayerCount = null;
            int? maximumPlayerCount = null;

            var optionSet = new OptionSet {
                {"n|name=", "The {NAME} of the new game", n => name = n},
                {"t|gameType=", "The {TYPE} of the new game", t => gameType = t},
                {"i|minimumPlayerCount=", "The {MINIMUM_PLAYER_COUNT} of the new game", i => minimumPlayerCount = SafeParse(i)},
                {"a|maximumPlayerCount=", "The {MAXIMUM_PLAYER_COUNT} of the new game", a => maximumPlayerCount = SafeParse(a)}
            };
            var extraParameters = optionSet.Parse(options);

            if (string.IsNullOrEmpty(name) || !minimumPlayerCount.HasValue || !maximumPlayerCount.HasValue || extraParameters.Count > 0)
            {
                new ErrorTextWriter(outputWriter).WriteUsage(Name, optionSet);
                return null;
            }

            return () => Execute(name, gameType, minimumPlayerCount.Value, maximumPlayerCount.Value);
        }

        private static int? SafeParse(string value)
        {
            if (int.TryParse(value, out var result))
            {
                return result;
            }
            return null;
        }

        private void Execute(string name, string gameType, int minimumPlayerCount, int maximumPlayerCount)
        {
            gameService.AddNewGame(name, gameType, minimumPlayerCount, maximumPlayerCount);
        }
    }
}