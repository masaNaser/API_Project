using KASHOP.BLL.Common;
using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Authentication
{
    // رابع خطوة نعمل السيرفس
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }


        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            // بنفحص أولاً إذا
            // الـ
            // user
            // نل،
            // أو إذا كلمة المرور غلط
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new LoginResponse()
                {
                    Message = "Invalid email or password"
                };
            }
            if(user.Id != null && !await _userManager.IsEmailConfirmedAsync(user))
            {
                return new LoginResponse()
                {
                    Message = "Email is not confirmed"
                };
            }
            return new LoginResponse()
            {
                Message = "Success",
                AccessToken = await GenerateJwt(user)

            };
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {


            var user = request.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                //ليش عملنا توكن؟ 
                //
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // 2. تشفير الـ Token حتى لا يخرب شكله أثناء التمرير في الـ URL
                var encodedToken = Uri.EscapeDataString(token);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "KASHOP.BLL", "Templates", "WelcomeEmail.html");                //  قراءة محتوى الملف
                var htmlTemplate = await File.ReadAllTextAsync(filePath);
                //  استبدال القيم المتغيرة داخل القالب
                var emailBody = htmlTemplate
            .Replace("{UserName}", user.UserName)
            .Replace("{UserId}", user.Id)
             .Replace("{Token}", encodedToken);

                await _emailSender.SendEmailAsync(user.Email, "Welcome to KASHOP", emailBody);


                return new RegisterResponse()
                {
                    Message = "Success",
                    UserId = user.Id,
                    Email = user.Email,
                    UserName = user.UserName
                };
            }
            return new RegisterResponse()
            {
                Message = "Error",
                Errors = result.Errors.Select(e => e.Description)

            };
        }

        private async Task<string> GenerateJwt(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            //المعلومات اللي رح تكون ضمن التوكن 
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, string.Join(",", roles))
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["ApiSettings:SecretKey"])); 
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["ApiSettings:issuer"],
                audience: _configuration["ApiSettings:audience"],
                claims: userClaims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }



        public async Task<bool> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x=>x.Id == request.UserId);
            if (user != null)
            {
                var token = Uri.UnescapeDataString(request.Token);
                var result = await _userManager.ConfirmEmailAsync(user,token);
                return result.Succeeded;
            }
            else
            {
                return false;
            }
        }
    }
}
