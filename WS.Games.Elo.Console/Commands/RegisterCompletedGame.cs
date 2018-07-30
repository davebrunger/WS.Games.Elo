using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NDesk.Options;
using WS.Games.Elo.Lib.Services;
using WS.Utilities.Console;

namespace WS.Games.Elo.Console.Commands
{
    public class RegisterCompletedGame : ICommand
    {
        private readonly IOutputWriter outputWriter;
        private readonly GameService gameService;

        public string Name => "register-completed-game";

        public RegisterCompletedGame(IOutputWriter outputWriter, GameService gameService)
        {
            this.outputWriter = outputWriter;
            this.gameService = gameService;
        }

        public Action GetAction(IEnumerable<string> options)
        {
            string name = null;
            DateTime? startTime = null;
            string location = null;

            var optionSet = new OptionSet {
                {"n|name=", "The {NAME} of game played", n => name = n},
                {"s|startTime=", "The {START_TIME} of the game", s => startTime = SafeParse(s)},
                {"l|location=", "The {LOCATION} where the game was played", l => location = l}
            };
            var extraParameters = optionSet.Parse(options);

            if (string.IsNullOrEmpty(name) || !startTime.HasValue || string.IsNullOrEmpty(location) || extraParameters.Count < 1)
            {
                new ErrorTextWriter(outputWriter).WriteUsage(Name, optionSet, "<winning-player> [<second-place-player> [...]]");
                return null;
            }

            return () => Execute(name, startTime.Value, location, extraParameters);
        }

        private static DateTime? SafeParse(string value)
        {
            if (DateTime.TryParseExact(value, "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }
            return null;
        }

        private void Execute(string name, DateTime startTime, string location, IEnumerable<string> playerNamesByPosition)
        {
            var groupedPlayerNamesByPosition = playerNamesByPosition
                .Select(pn => new List<string> { pn })
                .ToList();
            gameService.RegisterCompletedGame(name, startTime, location, groupedPlayerNamesByPosition);
        }
    }
}