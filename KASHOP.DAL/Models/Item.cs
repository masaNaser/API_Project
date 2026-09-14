using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice => (BasePrice - (BasePrice * Discount / 100));
        // dirived attribute
        public List<Product> Products { get; set; }
        //public Lazy<List<ItemTranslation>> Translations { get; set; };

    }
}
