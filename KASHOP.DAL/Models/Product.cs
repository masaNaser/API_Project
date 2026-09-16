namespace KASHOP.DAL.Models
{
    public class Product : AuditableEntity
    {
        public int Id { get; set; }
        public decimal BasePrice { get; set; }        
        public decimal Discount { get; set; }        
        public decimal FinalPrice => BasePrice - (BasePrice * Discount / 100); 
        public int Quantity { get; set; }            
        public double Rating { get; set; }           
        // الصور
        public string MainImage { get; set; }         
        public List<ProductImage> SubImages { get; set; }

        // حالة العرض
        public bool IsActive { get; set; } = true;

        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductTranslations> Translations { get; set; }
    }
}