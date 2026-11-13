namespace KASHOP.DAL.Dto.Response
{
    public class CartItemResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        // توحيد الحقول لتطابق Product
        public decimal BasePrice { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice { get; set; }

        public int Count { get; set; }
        public decimal TotalPrice => FinalPrice * Count; // السعر الإجمالي يعتمد على السعر النهائي مضروباً بالعدد

        public string MainImage { get; set; }
        public bool IsInStock { get; set; }
    }
}