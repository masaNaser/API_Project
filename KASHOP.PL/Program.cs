
using KASHOP.BLL.Services.Category;
using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository.Category;
using KASHOP.BLL.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using System.Globalization;
using KASHOP.BLL.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using KASHOP.BLL.Mapping;

namespace KASHOP.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
            builder.Services.AddScoped<ICategoryServices,CategoryServices>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IEmailSender,EmailSender>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddLocalization(options => options.ResourcesPath = "");
            const string defaultCulture = "en";
            var  supportedCulture = new[] { 
                new CultureInfo(defaultCulture),
                new CultureInfo("ar"),
            };
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCulture;
                options.SupportedUICultures = supportedCulture;
            });

            //«·ŒÿÊ… «·À«·À… ‰÷Ì› «·”Ì—›”
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                //Â«Ì ‰÷Ì›Â« Õ Ï ÌﬂÊ‰ «·«Ì„Ì· ›—Ìœ ·ﬂ· „” Œœ„ «‰Â „„‰Ê⁄ ÌﬂÊ‰ ›Ì Õ”«»Ì‰ „‰ ‰›” «·«Ì„Ì· 
                // »œ· „« ‰Õÿ ﬂÊœ Ê‰›Õ’ «·«ÌœÌ‰ Ì Ì „Ê›—… Â«Ì «·„Ì“… 
                options.User.RequireUniqueEmail = true;
            }).
                AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


            builder.Services.AddAuthentication(options =>
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
                    ValidIssuer = builder.Configuration["ApiSettings:issuer"],
                    ValidAudience = builder.Configuration["ApiSettings:audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["ApiSettings:SecretKey"]))
                };
            });


            var app = builder.Build();
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            MapsterConfig.MapsterConfigRegister();
            
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
