using System;
using System.Collections.Generic;
using NDesk.Options;
using WS.Games.Elo.Lib.Services;
using WS.Utilities.Console;

namespace WS.Games.Elo.Console.Commands
{
    public class AddNewPlayer : ICommand
    {
        private readonly IOutputWriter outputWriter;
        private readonly PlayerService playerService;

        public string Name => "add-new-player";

        public AddNewPlayer(IOutputWriter outputWriter, PlayerService playerService)
        {
            this.outputWriter = outputWriter;
            this.playerService = playerService;
        }

        public Action GetAction(IEnumerable<string> options)
        {
            string name = null;

            var optionSet = new OptionSet {
                {"n|name=", "The {NAME} of the new player", n => name = n}
            };
            var extraParameters = optionSet.Parse(options);

            if (string.IsNullOrEmpty(name)|| extraParameters.Count > 0)
            {
                new ErrorTextWriter(outputWriter).WriteUsage(Name, optionSet);
                return null;
            }

            return () => Execute(name);
        }

        private void Execute(string name)
        {
            playerService.AddNewPlayer(name);
        }
    }
}