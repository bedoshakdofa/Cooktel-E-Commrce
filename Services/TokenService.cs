using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cooktel_E_commrece.Services
{

    public class TokenService:ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
            
        }
        public string GetAccessToken(User user)
        {
            var key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtConfig:Key"]));
            var TokenValidityMin = _config.GetValue<int>("JwtConfig:TokenValidityMins");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserID.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
            };
            var TokenDescriptor = new SecurityTokenDescriptor
            {

                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512),
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(TokenValidityMin)
            };

            var TokenHandler = new JwtSecurityTokenHandler();
            return TokenHandler.WriteToken(TokenHandler.CreateToken(TokenDescriptor));
        }
    }
}
