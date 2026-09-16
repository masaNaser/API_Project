using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<Result<RegisterResponse>>RegisterAsync(RegisterRequest request);
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
        Task<Result<bool>> ConfirmEmailAsync(ConfirmEmailRequest request);
    }

}
