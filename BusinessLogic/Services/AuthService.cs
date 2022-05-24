using DataAccessLayer.Repository;
using DataAccessLayer.Repository.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SquaresAPI.BusinessLogic;
using SquaresAPI.DataAcccessLayer.Repository;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private IUserRepository repository;
        private IRefreshTokenRepository tokenrepository;
        private readonly JwtSettings _appSettings;

        public AuthService(IOptions<JwtSettings> appSettings, IUserRepository userRepository, IRefreshTokenRepository tokenRepository)
        {
            _appSettings = appSettings.Value;
            this.repository = userRepository;
            this.tokenrepository = tokenRepository;
        }

        public async Task<TokenResponse> AuthenticateAsync(AuthenticateRequest model)
        {
            var byteArraypswd = new UTF8Encoding().GetBytes(model.Password);
            string pswd_encoded = Convert.ToBase64String(byteArraypswd);
            var user = await repository.ValidateUser(model.Username, pswd_encoded);
            if (user == null) return null;
            var jwttoken = generateJwtToken(user.UserId, model.Username);
            var refreshtoken = generateRefreshToken();
            RefreshToken newRefreshtoken = new RefreshToken() { IsActive = true, Refresh_Token = refreshtoken, UserId = user.UserId };
            await tokenrepository.AddOrUpdateAsync(newRefreshtoken);
            return new TokenResponse(user.UserName, jwttoken.Item1, jwttoken.Item2, refreshtoken);
        }

        public async Task<TokenResponse> RefreshToken(TokenResponse token)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var principle = GetPrincipalFromExpiredToken(token.token);
            Claim claim = ((ClaimsIdentity)principle.Identity).Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            int.TryParse(claim.Value, out int userid);
            RefreshToken existingRefreshtoken = new RefreshToken() { Refresh_Token = token.refresh_token, UserId = userid };
            var result = tokenrepository.FindToken(existingRefreshtoken);
            if (result == null)
                return null;
            var jwttoken = generateJwtToken(userid, token.username);
            var newrefreshtoken = generateRefreshToken();
            result.Refresh_Token = newrefreshtoken;
            await tokenrepository.SaveChangesAsync();
            TokenResponse tokenResponse = new TokenResponse(token.username,jwttoken.Item1, jwttoken.Item2, newrefreshtoken);
            string str = JsonConvert.SerializeObject(tokenResponse);
            return tokenResponse;
        }

        private Tuple<DateTime, string> generateJwtToken(int userId, string Username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            DateTime expires = DateTime.Now.AddMinutes(_appSettings.ExpirationTimeInMinutes);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, Username),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())}),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwttoken= tokenHandler.WriteToken(token);
            return Tuple.Create(expires, jwttoken);
        }

        private string generateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var randomnumGenerator = RandomNumberGenerator.Create())
            {
                randomnumGenerator.GetBytes(randomNumber);
                string refreshtoken = Convert.ToBase64String(randomNumber);
                return refreshtoken;
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var Key = Encoding.UTF8.GetBytes(_appSettings.Secret);
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid Token");
                }
                return principal;
            }
            catch (Exception ex) { throw new SecurityTokenException("Invalid Token"); }
        }
    }
}
