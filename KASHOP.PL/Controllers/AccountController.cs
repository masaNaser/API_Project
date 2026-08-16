using KASHOP.BLL.Services.Authentication;
using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService )
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            return Ok(result);
        }
        [HttpPost("register")]

        public async Task <IActionResult> Register(RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request);
            return Ok(result);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Message = "Email is required." });
            }
            var isConfirmed = await _authenticationService.ConfirmEmailAsync(email);

            if (!isConfirmed)
            {
                return BadRequest(new { Message = "User not found or confirmation failed." });
            }
            return Ok(new { Message = "Email Confirmed successfully." });
        }

    }
}
