using System;
using System.Collections.Generic;
using System.Linq;
using NDesk.Options;
using WS.Games.Elo.Lib.Services;
using WS.Utilities.Console;
using WS.Utilities.Console.Tabulation;

namespace WS.Games.Elo.Console.Commands
{
    public class ShowPlayerRankings : ICommand
    {
        private readonly IOutputWriter outputWriter;
        private readonly PlayerService playerService;

        public string Name => "show-player-rankings";

        public ShowPlayerRankings(IOutputWriter outputWriter, PlayerService playerService)
        {
            this.outputWriter = outputWriter;
            this.playerService = playerService;
        }

        public Action GetAction(IEnumerable<string> options)
        {
            int? minimumNumberOfGames = null;

            var optionSet = new OptionSet {
                {
                    "g|minimumNumberOfGames=",
                    "The {MINIMUM_NUMBER_OF_GAMES} a player needs to have played in order to be included in the ranking",
                    g => minimumNumberOfGames = int.Parse(g)
                }
            };
            try
            {
                var extraParameters = optionSet.Parse(options);
                if (extraParameters.Count > 0)
                {
                    throw new Exception();
                }
            }
            catch
            {
                new ErrorTextWriter(outputWriter).WriteUsage(Name, optionSet);
                return null;
            }

            return () => Execute(minimumNumberOfGames ?? 1);
        }

        private void Execute(int minimumNumberOfGames)
        {
            playerService.Get(minimumNumberOfGames: minimumNumberOfGames)
                .OrderByDescending(r => r.Rating)
                .Tabulate(outputWriter, true, 5);
        }
    }
}