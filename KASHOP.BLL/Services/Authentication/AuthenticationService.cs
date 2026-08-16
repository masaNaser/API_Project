using KASHOP.BLL.Common;
using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Authentication
{
    // رابع خطوة نعمل السيرفس
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager=userManager;
            _emailSender = emailSender;
        }

        public async Task<bool> ConfirmEmailAsync(string email)
        {
            var user =await _userManager.Users.FirstOrDefaultAsync(e => e.Email == email);
            if (user != null)
            {
                // 2. تغيير حالة تأكيد البريد إلى true
                user.EmailConfirmed = true;

                // 3. حفظ التعديلات في قاعدة البيانات
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            else
            {
                return false;
            }
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

            return new LoginResponse()
            {
                Message = "Success"
            };
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {


            var user = request.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(user,request.Password);

            if (result.Succeeded)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "KASHOP.BLL", "Templates", "WelcomeEmail.html");                //  قراءة محتوى الملف
                var htmlTemplate = await File.ReadAllTextAsync(filePath);
                //  استبدال القيم المتغيرة داخل القالب
                var emailBody = htmlTemplate
            .Replace("{UserName}", user.UserName)
            .Replace("{Email}", user.Email);
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
    }
}
