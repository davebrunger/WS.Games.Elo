using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using WS.Games.Elo.Lib.Services;

namespace WS.Games.Elo.Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly GameService gameService;

        public GamesController(GameService gameService)
        {
            this.gameService = gameService;
        }

        [HttpGet()]
        [Route("recent/{numberOfGames}")]
        public IActionResult GetRecentGames(int? numberOfGames)
        {
            var gameResults = gameService.GetRecentGames(numberOfGames ?? 5);
            return Ok(gameResults);
        }

        [HttpGet()]
        [Route("{gameName}")]
        public IActionResult GetGame(string gameName)
        {
            var game = gameService.GetGame(gameName);
            return Ok(game);
        }

        [HttpGet()]
        [Route("{gameName}/thumbnail")]
        public IActionResult GetGameThumbnail(string gameName)
        {
            var imageBytes = gameService.GetGameThumbnail(gameName);
            return File(imageBytes, "image/png");
        }
    }
}