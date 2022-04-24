namespace ShopAPI.Models.Forms
{
    public class AddProductForm
    {
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
