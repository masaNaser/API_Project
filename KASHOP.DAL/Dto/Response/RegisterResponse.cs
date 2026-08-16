using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KASHOP.DAL.Dto.Response
{
    public class RegisterResponse
    {
        // رسالة تأكيد للواجهة الأمامية (Frontend)
        public string Message { get; set; }
        //public string Token { get; set; };
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Email { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UserName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string>? Errors { get; set; } 



        //توضيح للشرط
        //    :"لو القيمة المخرجة لل
        //    Property دي 
        //    بتساوي
        //    null،
        //    احذف الحقل تماماً من 
        //    الـ
        //    JSON
        //    النهائي
        //    وما تظهره للمستخدم.
    }
}
