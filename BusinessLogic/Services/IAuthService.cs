using SquaresAPI.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IAuthService
    {
        Task<TokenResponse> AuthenticateAsync(AuthenticateRequest model);
        Task<TokenResponse> RefreshToken(TokenResponse token);
    }
}
