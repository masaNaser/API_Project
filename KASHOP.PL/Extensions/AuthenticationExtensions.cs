using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace KASHOP.PL.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthenticationServices(this IServiceCollection services ,IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
      .AddJwtBearer(options =>
      {
          options.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              ValidIssuer =configuration["ApiSettings:issuer"],
              ValidAudience =configuration["ApiSettings:audience"],
              IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["ApiSettings:SecretKey"]))
          };
      });
            services.AddAuthorization();
            return services;

        }
    }
}
