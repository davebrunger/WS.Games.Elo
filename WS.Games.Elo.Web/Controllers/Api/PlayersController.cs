using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WS.Games.Elo.Lib.Services;

namespace WS.Games.Elo.Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class PlayersController : Controller
    {
        private readonly PlayerService playerService;

        public PlayersController(PlayerService playerService)
        {
            this.playerService = playerService;
        }

        [HttpGet()]
        public IActionResult GetAllPlayers(int? minimumNumberOfGames)
        {
            var players = playerService.Get(minimumNumberOfGames)
                .OrderBy(p => p.Name);
            return Ok(players);
        }
    }
}
