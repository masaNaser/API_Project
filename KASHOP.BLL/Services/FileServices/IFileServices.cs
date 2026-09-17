using KASHOP.DAL.Dto.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.FileServices
{
    public interface IFileServices
    {
        Task<Result<string>> UploadFileAsync(IFormFile file);
    }
}
