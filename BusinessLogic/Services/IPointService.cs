using BusinessLogic.Models;
using SquaresAPI.BusinessLogic;
using System.Drawing;

namespace SquaresAPI.Services
{
    public interface IPointService
    {
        /// <summary>
        /// Add a new point to Repository. 
        /// </summary>
        /// <param name="point">Having X and Y co-ordinates</param>
        /// <returns>string, If point added successfully, then return true, if point already exists in DB, then returns-false</returns>
        Task<bool> AddPointAsync(PointModel point,int UserId);

        /// <summary>
        /// Delete a point from the Repository.
        /// </summary>
        /// <param name="point">Having X and Y co-ordinates</param>
        /// <returns></returns>
        Task DeletePointAsync(PointModel point, int UserId);

        /// <summary>
        /// Import an array of points into Repository
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        Task ImportPointsAsync(PointModel[] point, int UserId);

        /// <summary>
        /// Calculate the squares that can be made up from Points Repository.
        /// </summary>
        /// <returns>HashSet of Squares containing Point A,B,C,D.</returns>
        Task<HashSet<SquareModel>> GetSquaresAsync(int UserId);
    }
}
