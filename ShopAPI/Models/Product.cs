namespace ShopAPI.Models
{
    public class Product
    {
        public Product()
        {
        }
        public Product(int articleNumber, string productName, string description, decimal price, int quantity, string categoryName)
        {
            ArticleNumber = articleNumber;
            ProductName = productName;
            Description = description;
            Price = price;
            Quantity = quantity;
            CategoryName = categoryName;
        }

        public int ArticleNumber { get; set; }
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
