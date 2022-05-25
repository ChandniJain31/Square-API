using BusinessLogic.Models;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquaresAPI.BusinessLogic;
using SquaresAPI.Filters;

namespace SquaresAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(LogAttribute))]
    
    public class UserController : ControllerBase
    {
        private IAuthService _userService;
        
        public UserController(IAuthService  userService)
        { this._userService = userService; }

        [AllowAnonymous]
        [HttpPost("~/token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedError))]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _userService.AuthenticateAsync(model);
            if (response == null)
            {
                return Unauthorized(new UnauthorizedError("Authorization information is missing or invalid."));
            }
             return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("~/refreshtoken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type =typeof(UnauthorizedError))]
        public async Task<IActionResult> RefreshToken([FromBody]TokenResponse token)
        {
            var response =await _userService.RefreshToken(token);
            if (response == null)
            { return Unauthorized( new UnauthorizedError("Authorization information is missing or invalid.")); }
            return Ok(response);
        }      
    }
}
