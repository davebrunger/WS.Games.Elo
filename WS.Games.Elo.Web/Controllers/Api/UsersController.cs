using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WS.Games.Elo.Web.Services;

namespace WS.Games.Elo.Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly SecurityService securityService;

        public UsersController(SecurityService securityService)
        {
            this.securityService = securityService;
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var token = securityService.GetNewToken(username, password);
            if (token == null)
            {
                return BadRequest();
            }
            return Ok(new { Token = token });
        }

        [Authorize]
        [Route("current")]
        [HttpGet]
        public IActionResult GetCurrentUser()
        {
            return Ok(new { Name = User.Identity.Name });
        }
    }
}