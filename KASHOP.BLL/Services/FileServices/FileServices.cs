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
             if (file is null || file.Length == 0)
                {
                 
                    return Result<string>.FailureResult("No File Was Provided.");
                }
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_allowdExtensions.Contains(extension))
                {
                  
                    return Result<string>.FailureResult($"File Extension '{extension}' is Not Allowed.");
                }
                if (file.Length > _maxFileSize)
                {
                    // تحويل البايت إلى ميجابايت لعرض رسالة 
                    var maxMb = _maxFileSize / (1024 * 1024);
                    return Result<string>.FailureResult($"File Size Exceeds The Maximum Allowed Size of '{maxMb}'MB.");
                }
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","Images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
                //return new Result<string>
                //{
                //    Success = true,
                //    Message = "File Uploaded Successfully.",
                //    Data = fileName
                //};
                return Result<string>.SuccessResult(fileName, "File Uploaded Successfully.");
        
        }

        public Result<bool> DeleteFileAsync(string fileName)
        {            
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return Result<bool>.FailureResult("File name is empty.");
                }

                // الحماية من ثغرة Directory Traversal لتأمين المسار
                var safeFileName = Path.GetFileName(fileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", safeFileName);

                if (!File.Exists(filePath))
                {
                    return Result<bool>.FailureResult("File not found.");
                }

                File.Delete(filePath);
                return Result<bool>.SuccessResult(true, "File deleted successfully.");
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
