using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Dto.Response
{
    public class ProductListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MainImage { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice { get; set; }
        public double Rating { get; set; }
        public string BrandName { get; set; }
        public bool IsInStock { get; set; }
    }
}
