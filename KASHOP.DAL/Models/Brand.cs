using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    //البراند ممكن من خلاله انه اليوزر يعمل فلترة حسب الشركات وهيك 
    public class Brand :AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }    
        public List<Product> Products { get; set; }
    }
}
