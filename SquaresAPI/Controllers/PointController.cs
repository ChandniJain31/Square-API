using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquaresAPI.Services;
using SquaresAPI.BusinessLogic;
using System.Drawing;
using SquaresAPI.Filters;
using BusinessLogic.Models;
using System.Web.Http.ModelBinding;
using System.Net;

namespace SquaresAPI.Controllers
{
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(LogAttribute))]
    public class PointController : ControllerBase
    {
        private IPointService pointService;        
       
        public PointController(IPointService pointService)
        {
            this.pointService = pointService;
            
        }

        [Route("/GetSquares")]
        [HttpGet]       
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedError))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type =typeof(NotFoundError))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SquareResponse))]
        public async Task<IActionResult> GetSquaresAsync()
        {
            var squares = await pointService.GetSquaresAsync(User.GetLoggedInUserId<int>());
            if (squares == null || squares.Count == 0)
            {
                return NotFound(new NotFoundError("No squares can be generated."));
            }
            return Ok(new SquareResponse(){ count = squares.Count, data = squares });
        }

        [HttpPost]
        [Route("/Point/Add")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(BadRequestError))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddAsync([FromBody] PointModel point)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var issuccess = await pointService.AddPointAsync(point,User.GetLoggedInUserId<int>());
            if (!issuccess)
            {
                return BadRequest(new BadRequestError("Point already exists."));
            }
            return Created(string.Empty, null);
        }

        [HttpPost]
        [Route("/Point/Import")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> ImportAsync([FromBody] PointModel[] points)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await pointService.ImportPointsAsync(points, User.GetLoggedInUserId<int>());
            return Created(String.Empty,null);
        }

        [HttpPost]
        [Route("/Point/Delete")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromBody] PointModel point)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await pointService.DeletePointAsync(point, User.GetLoggedInUserId<int>());
            return NoContent();
        }
        
    }
}
