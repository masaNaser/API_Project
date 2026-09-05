using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KASHOP.DAL.Dto.Response
{
    public class LoginResponse
    {
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AccessToken { get; set; }
        //public string RefreshToken { get; set; }
    }
}
