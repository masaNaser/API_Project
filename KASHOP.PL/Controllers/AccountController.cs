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
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            return Ok(result);
        }


        [HttpPost("register")]
        public async Task <IActionResult> Register([FromBody]RegisterRequest request)
        {
            var response  = await _authenticationService.RegisterAsync(request);
            // إذا كانت العملية تحتوي على أخطاء أو IsSuccess == false
            if (response.Errors!=null && response.Errors.Any()) // أو حسب الفحص لديك
            {
                return BadRequest(response); // سيرجع HTTP Status 400 Bad Request
            }
            return Ok(response);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]ConfirmEmailRequest request)
        {
            var isConfirmed = await _authenticationService.ConfirmEmailAsync(request);

            if (!isConfirmed)
            {
                return BadRequest(new { Message = "User not found or confirmation failed." });
            }
            return Ok(new { Message = "Email Confirmed successfully." });
        }

    }
}
