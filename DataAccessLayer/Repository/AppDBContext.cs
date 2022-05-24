using DataAccessLayer.Repository.Models;
using Microsoft.EntityFrameworkCore;
using SquaresAPI.DataAcccessLayer.Repository.Models;
namespace SquaresAPI.DataAcccessLayer.Repository;

public class AppDBContext:DbContext
{
    public AppDBContext(DbContextOptions options) : base(options) {}
    public DbSet<Points> Points
    {
        get;
        set;
    }
    public DbSet<User> Users
    {
        get;
        set;
    }
    public DbSet<RefreshToken> RefreshTokens
    {
        get;
        set;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId=1,
                UserName = "admin",
                Password = "YWRtaW4=" 
            },
            new User
            {
                UserId = 2,
                UserName = "user1",
                Password = "dXNlcjE=" 
            },
            new User
            {
                UserId = 3,
                UserName = "user2",
                Password = "dXNlcjI=" 
            }
        );
    }
}
