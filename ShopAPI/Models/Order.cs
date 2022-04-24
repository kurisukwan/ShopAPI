namespace ShopAPI.Models
{
    public class Order
    {
        public Order()
        {
        }
        public Order(int id, DateTime orderDate, string status, string userFirstName, string userLastName, string userEmail, ICollection<OrderItem> orderItems)
        {
            CartNr = id;
            OrderDate = orderDate;
            Status = status;
            UserFirstName = userFirstName;
            UserLastName = userLastName;
            UserEmail = userEmail;
            OrderItems = orderItems;
        }

        public int CartNr { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!;
        public string UserFirstName { get; set; } = null!;
        public string UserLastName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = null!;
    }
}
