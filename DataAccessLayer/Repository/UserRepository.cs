using Microsoft.EntityFrameworkCore;
using SquaresAPI.DataAcccessLayer.Repository.Models;

namespace SquaresAPI.DataAcccessLayer.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDBContext dBContext;
        public UserRepository(AppDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
       
        public async Task<User> ValidateUser(string Username, string Password)
        {
            return await dBContext.Users.Where(p => Username.Equals(p.UserName) && Password.Equals(p.Password)).FirstOrDefaultAsync();

        }

        public async Task<User> GetUserById(int id)
        {
            return await dBContext.Users.Where(x => x.UserId == id).FirstOrDefaultAsync();
        }
    }
}
