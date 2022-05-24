using SquaresAPI.DataAcccessLayer.Repository.Models;

namespace SquaresAPI.DataAcccessLayer.Repository
{
    public interface IPointRepository
    {
         Task<bool> CheckIfAlreadyExistAsync(Points point);
         Task<IEnumerable<Points>> GetAllAsync(int UserId);
         Task<Points> AddAsync(Points point);
         Task ImportAsync(Points[] points);
         Task DeleteAsync(Points point);
    }
}
