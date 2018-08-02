using System;
using System.Collections.Generic;
using NDesk.Options;
using WS.Games.Elo.Lib.Services;
using WS.Utilities.Console;

namespace WS.Games.Elo.Console.Commands
{
    public class SetGameThumbnail : ICommand
    {
        private readonly IOutputWriter outputWriter;
        private readonly ImageService imageService;
        private readonly GameService gameService;

        public SetGameThumbnail(IOutputWriter outputWriter, ImageService imageService, GameService gameService)
        {
            this.outputWriter = outputWriter;
            this.imageService = imageService;
            this.gameService = gameService;
        }

        public string Name => "set-game-thumbnail";

        public Action GetAction(IEnumerable<string> options)
        {
            string gameName = null;
            string thumbnailUrl = null;

            var optionSet = new OptionSet {
                {"n|gameName=", "The {NAME} of the game to set the thumbnail for", n => gameName = n},
                {"u|thumbnailUrl=", "The {URL} of the thumbnail", u => thumbnailUrl = u}
            };
            var extraParameters = optionSet.Parse(options);

            if (string.IsNullOrEmpty(gameName) || string.IsNullOrEmpty(thumbnailUrl) || extraParameters.Count > 0)
            {
                new ErrorTextWriter(outputWriter).WriteUsage(Name, optionSet);
                return null;
            }

            return () => Execute(gameName, thumbnailUrl);
        }

        private void Execute(string gameName, string thumbnailUrl)
        {
            var imageBytes = imageService.GetImageBytesAsync(thumbnailUrl).Result;
            var resizedImageBytes = imageService.ResizeImage(imageBytes, 100);
            gameService.SetGameThumbnail(gameName, resizedImageBytes);
        }
    }
}