using Microsoft.EntityFrameworkCore;
using SquaresAPI.DataAcccessLayer.Repository.Models;

namespace SquaresAPI.DataAcccessLayer.Repository
{
    public class PointRepository:IPointRepository
    {
        private readonly AppDBContext dBContext;
      
        public PointRepository(AppDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
    
        public async Task<bool> CheckIfAlreadyExistAsync(Points point)
        {
            return await dBContext.Points.AnyAsync(p => p.X == point.X && p.Y == point.Y && p.UserId==point.UserId);
        }
        
        public async Task<IEnumerable<Points>> GetAllAsync(int UserId)
        {
            return await dBContext.Points.Where(x => x.UserId == UserId).ToListAsync();
        }

        public async Task<Points> AddAsync(Points point)
        {
            var alreadyexist = await CheckIfAlreadyExistAsync(point);
            if (!alreadyexist)
            {
                var result = await dBContext.Points.AddAsync(point);
                await dBContext.SaveChangesAsync();
                return result.Entity;
            }
            else return null;
        }

        public async Task ImportAsync(Points[] points)
        {
            var notfoundpoints = points.Where(a => !dBContext.Points.Any(pt => a.X == pt.X && a.Y == pt.Y && a.UserId == pt.UserId)).ToHashSet();
            await dBContext.Points.AddRangeAsync(notfoundpoints);
            //await dBContext.Points.AddRangeAsync(points.Where(a => !dBContext.Points.Any(pt => a.X == pt.X && a.Y == pt.Y && a.UserId == pt.UserId)));
            await dBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Points point)
        {
            var result = await dBContext.Points
                .FirstOrDefaultAsync(e => e.X == point.X && e.Y == point.Y && e.UserId==point.UserId);
            if (result != null)
            {
                 dBContext.Points.Remove(result);
                await dBContext.SaveChangesAsync();
            }
        }
    }
}
