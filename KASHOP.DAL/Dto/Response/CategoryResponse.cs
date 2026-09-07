using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Dto.Response
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string User { get; set; }
        public string Name { get; set; }
        //public List<CategoryTranslationResponse> Translations { get; set; }
    }
}
