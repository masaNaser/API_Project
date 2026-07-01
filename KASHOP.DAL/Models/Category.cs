using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public List<CatagoryTranslations> Translations { get; set; } = new();
    }
}
