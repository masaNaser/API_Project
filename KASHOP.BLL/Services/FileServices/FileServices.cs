using KASHOP.DAL.Dto.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.FileServices
{
    public class FileServices : IFileServices
    {
        private readonly string[] _allowdExtensions = { ".jpg", ".jpeg", ".png",".webp" };
        private const long _maxFileSize = 5 * 1024 * 1024; // 5 MB
        public async Task<Result<string>>UploadFileAsync(IFormFile file)
        {
            try
            {
                if (file is null || file.Length == 0)
                {
                    return new Result<string>
                    {
                        Success = false,
                        Message = "No File Was Provided."
                    };
                }
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_allowdExtensions.Contains(extension))
                {
                    return new Result<string>
                    {
                        Success = false,
                        Message = $"File Extension '{extension}' is Not Allowed."
                    };
                }
                if (file.Length > _maxFileSize)
                {
                    // تحويل البايت إلى ميجابايت لعرض رسالة 
                    var maxMb = _maxFileSize / (1024 * 1024);
                    return new Result<string>
                    {
                        Success = false,
                        Message = $"File Size Exceeds The Maximum Allowed Size of '{maxMb}'MB."
                    };
                }
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "/Images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
                return new Result<string>
                {
                    Success = true,
                    Message = "File Uploaded Successfully.",
                    Data = fileName
                };
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return new Result<string>
                {
                    Success = false,
                    Message = $"An Error Occurred While Uploading The File: {errorMessage}"
                };
            }
        }
    }
    //if (file is not null && file.Length > 0) {
    //    //احتجنا هون انه نشفر اسم الملف اللي رح يتم رفعه 
    //    // لانه ممكن اكثر من يوزر يرفع نفس الملف بنفس الاسم،
    //    // فهون رح نستخدم
    //    // Guid
    //    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    //    //تحديد مسار الحفظ على السيرفر
    //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Images", fileName);
    //    //هاد الجزء مسؤول عن رفع الملف على السيرفر
    //    using (var stream = System.IO.File.Create(filePath))
    //    {
    //        await file.CopyToAsync(stream);
    //    }
    //    return fileName;
    //}
    //return null;

}
