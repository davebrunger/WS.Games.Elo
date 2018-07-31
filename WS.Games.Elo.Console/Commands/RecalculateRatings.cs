using System;
using System.Collections.Generic;
using System.Linq;
using WS.Games.Elo.Lib.Services;
using WS.Utilities.Console;

namespace WS.Games.Elo.Console.Commands
{
    public class RecalculateRatings : ICommand
    {
        private readonly IOutputWriter outputWriter;
        private readonly GameService gameService;

        public string Name => "recalculate-ratings";

        public RecalculateRatings(IOutputWriter outputWriter, GameService gameService)
        {
            this.outputWriter = outputWriter;
            this.gameService = gameService;
        }

        public Action GetAction(IEnumerable<string> options)
        {
            if (options.Count() > 0)
            {
                outputWriter.WriteErrorLine("Usage:");
                outputWriter.WriteErrorLine($"dotnet run -- recalculate-ratings");
                return null;
            }
            return () => Execute();
        }

        private void Execute()
        {
            gameService.RecalculateRatings();
        }
    }
}