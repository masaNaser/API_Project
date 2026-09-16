using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Dto.Response
{
    public class ProductDetailsResponse: ProductListResponse
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; }
        public string BrandLogo { get; set; }
        public List<string> SubImages { get; set; }
    }
}
