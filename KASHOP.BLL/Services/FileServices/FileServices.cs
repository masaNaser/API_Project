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
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file is not null && file.Length > 0) {
                //احتجنا هون انه نشفر اسم الملف اللي رح يتم رفعه 
                // لانه ممكن اكثر من يوزر يرفع نفس الملف بنفس الاسم،
                // فهون رح نستخدم
                // Guid
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                //تحديد مسار الحفظ على السيرفر
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Images", fileName);
                //هاد الجزء مسؤول عن رفع الملف على السيرفر
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
                return fileName;
            }
            return null;
        }
    }
}
