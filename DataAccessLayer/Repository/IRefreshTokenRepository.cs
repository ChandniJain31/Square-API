using DataAccessLayer.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public interface IRefreshTokenRepository
    {        
        Task<RefreshToken> AddAsync(RefreshToken token);
        Task<RefreshToken> AddOrUpdateAsync(RefreshToken token);
        RefreshToken FindToken(RefreshToken token);
        Task DeleteAsync(int userid, string refreshToken);
        Task SaveChangesAsync();
    }
}
