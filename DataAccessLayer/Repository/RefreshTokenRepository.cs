using DataAccessLayer.Repository.Models;
using SquaresAPI.DataAcccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDBContext dBContext;

        public RefreshTokenRepository(AppDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<RefreshToken> AddAsync(RefreshToken token)
        {
            var result = await dBContext.RefreshTokens.AddAsync(token);
            await SaveChangesAsync();
            return result.Entity;
        }
        public async Task<RefreshToken> AddOrUpdateAsync(RefreshToken token)
        {
            var existingtoken=  dBContext.RefreshTokens.FirstOrDefault<RefreshToken>(x=>x.UserId==token.UserId);
            if (existingtoken != null)
            {
                existingtoken.Refresh_Token = token.Refresh_Token;
                await SaveChangesAsync();
            }
            else existingtoken= await AddAsync(token);
            return existingtoken;
        }
        public RefreshToken FindToken(RefreshToken token)
        {
            RefreshToken existingtoken =dBContext.RefreshTokens.FirstOrDefault<RefreshToken>(x => x.UserId == token.UserId && x.Refresh_Token.Equals(token.Refresh_Token));
            return existingtoken;
        }
        public async Task SaveChangesAsync()
        {
           await dBContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int userid, string refreshToken)
        {
            var existingtoken = dBContext.RefreshTokens.FirstOrDefault<RefreshToken>(x => x.UserId == userid && x.Refresh_Token.Equals(refreshToken));
            dBContext.RefreshTokens.Remove(existingtoken);
            await SaveChangesAsync();

        }
    }
}
