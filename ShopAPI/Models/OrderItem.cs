namespace ShopAPI.Models
{
    public class OrderItem
    {
        public OrderItem()
        {
        }

        public OrderItem(string productName, string description, decimal price, int quantity)
        {
            ProductName = productName;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        public int Id { get; set; }
        public int ArticleNumber { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
