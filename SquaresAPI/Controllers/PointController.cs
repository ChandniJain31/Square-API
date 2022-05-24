using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquaresAPI.Services;
using SquaresAPI.BusinessLogic;
using System.Drawing;
using SquaresAPI.Filters;

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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SquareResponse))]
        public async Task<IActionResult> GetSquaresAsync()
        {
            var squares = await pointService.GetSquaresAsync(User.GetLoggedInUserId<int>());
            return (squares == null || squares.Count == 0) ? NotFound("No squares can be generated!"):Ok(new SquareResponse(){ count = squares.Count, data = squares });
        }

        [HttpPost]
        [Route("/Point/Add")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddAsync([FromBody] Point point)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var msg = await pointService.AddPointAsync(point,User.GetLoggedInUserId<int>());
            return string.IsNullOrEmpty(msg) ? Created(String.Empty, null) : BadRequest(msg);
        }

        [HttpPost]
        [Route("/Point/Import")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> ImportAsync([FromBody] Point[] points)
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync([FromBody]Point point)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await pointService.DeletePointAsync(point, User.GetLoggedInUserId<int>());
            return NoContent();
        }
        
    }
}
