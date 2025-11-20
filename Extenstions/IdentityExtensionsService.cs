using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace Cooktel_E_commrece.Extenstions
{
    public static class  IdentityExtensionsService
    {
        public static IServiceCollection AddJwtIdentity(this IServiceCollection services, IConfiguration config) {

            services.AddAuthentication(opt => opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => opt.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtConfig:Key"])),
                    ValidateIssuer=false,
                    ValidateAudience=false,
                    ValidateIssuerSigningKey=true,
                    ValidateLifetime=true,
                    ClockSkew=TimeSpan.Zero,
                });

            return services;
        }
    }
}
