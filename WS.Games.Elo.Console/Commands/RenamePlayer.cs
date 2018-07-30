using System;
using System.Collections.Generic;
using NDesk.Options;
using WS.Games.Elo.Lib.Services;
using WS.Utilities.Console;

namespace WS.Games.Elo.Console.Commands
{
    public class RenamePlayer : ICommand
    {
        private readonly IOutputWriter outputWriter;
        private readonly PlayerService playerService;

        public string Name => "rename-player";

        public RenamePlayer(IOutputWriter outputWriter, PlayerService playerService)
        {
            this.outputWriter = outputWriter;
            this.playerService = playerService;
        }

        public Action GetAction(IEnumerable<string> options)
        {
            string oldName = null;
            string newName = null;

            var optionSet = new OptionSet {
                {"o|oldName=", "The {OLD_NAME} of the player to be renamed", o => oldName = o},
                {"n|newName=", "The {NEW_NAME} of the player to be renamed", n => newName = n}
            };
            var extraParameters = optionSet.Parse(options);

            if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName) || extraParameters.Count > 0)
            {
                new ErrorTextWriter(outputWriter).WriteUsage(Name, optionSet);
                return null;
            }

            return () => Execute(oldName, newName);
        }

        private void Execute(string oldName, string newName)
        {
            playerService.RenamePlayer(oldName, newName);
        }
    }
}