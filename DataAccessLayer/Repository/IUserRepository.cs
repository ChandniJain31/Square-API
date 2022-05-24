using SquaresAPI.DataAcccessLayer.Repository.Models;

namespace SquaresAPI.DataAcccessLayer.Repository
{
    public interface IUserRepository
    {
        Task<User> ValidateUser(string Username, string Password);
        Task<User> GetUserById(int id);
    }
}
