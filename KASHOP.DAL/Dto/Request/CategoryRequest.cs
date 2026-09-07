using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Dto.Request
{
    public class CategoryRequest
    {
        public string Image { get; set; }
        public List<CategoryTranslationRequest> Translations { get; set; }
    
    }
}
