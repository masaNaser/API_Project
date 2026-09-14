using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    public class Product : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public string MainImage { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public List<ProductTranslations> Translations { get; set; }
    }
}
