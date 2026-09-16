using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Dto.Request
{
    public class ProductDto
    {
        public decimal BasePrice { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public string MainImage { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public List<ProductTranslationRequest> Translations { get; set; }
    }
}
