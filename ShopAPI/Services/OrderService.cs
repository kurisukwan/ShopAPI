using Microsoft.EntityFrameworkCore;
using ShopAPI.Data;
using ShopAPI.Data.Entities.OrderEntities;
using ShopAPI.Models;
using ShopAPI.Models.Forms;

namespace ShopAPI.Services
{
    public interface IOrderService
    {
        Task<Order> CreateAsync(CreateOrderForm form);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetAsync(int id);
        Task<Order> UpdateAsync(int id, UpdateOrderStatusForm form);
        Task<bool> DeleteAsync(int id);
    }
    public class OrderService : IOrderService
    {
        private readonly DataContext context;
        private readonly IOrderItemService orderItemService;

        public OrderService(DataContext context, IOrderItemService orderItemService)
        {
            this.context = context;
            this.orderItemService = orderItemService;
        }

        // Create-----------------------------------------------------------------------------------
        public async Task<Order> CreateAsync(CreateOrderForm form)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == form.Email);
            if (user != null)
            {
                var orderEntity = new OrderEntity
                {
                    OrderDate = DateTime.Now,
                    Status = "Not yet confirmed",
                    UserId = user.Id
                };
                context.Orders.Add(orderEntity);
                await context.SaveChangesAsync();
                return new Order(orderEntity.Id, orderEntity.OrderDate, orderEntity.Status, user.FirstName,
                    user.LastName, user.Email, new List<OrderItem>());
            }
            return null!;
        }

        // ReadAll----------------------------------------------------------------------------------
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var orders = new List<Order>();
            foreach (var order in await context.Orders.Include(x => x.User).Include(x => x.OrderItems).ToListAsync())
                orders.Add(new Order
                {
                    CartNr = order.Id,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    UserFirstName = order.User.FirstName,
                    UserLastName = order.User.LastName,
                    UserEmail = order.User.Email,
                    OrderItems = (ICollection<OrderItem>) await orderItemService.GetAllAsync(order.Id)
                });
            return orders; 
        }

        // Read-------------------------------------------------------------------------------------
        public async Task<Order> GetAsync(int id)
        {
            var order = await context.Orders.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
            if (order != null)
                return new Order(order.Id, order.OrderDate, order.Status, order.User.FirstName,
                order.User.LastName, order.User.Email, (ICollection<OrderItem>) await orderItemService.GetAllAsync(order.Id));
            return null!;
        }

        // Update-----------------------------------------------------------------------------------

        // En update av ordern är i princip en CREATE, UPDATE och DELETE av en OrderItem kombinerat.
        // Därför blir det smidigare att skapa ytterligare en controller som hanterar just dessa 3.
        // Update av ordern innebär en ändring av order statusen istället.
        public async Task<Order> UpdateAsync(int id, UpdateOrderStatusForm form)
        {
            var order = await context.Orders.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
            if (order != null)
            {
                context.Entry(order).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return new Order(order.Id, order.OrderDate, form.Status, order.User.FirstName,
                order.User.LastName, order.User.Email, new List<OrderItem>());
            }

            return null!;
        }

        // Delete-----------------------------------------------------------------------------------
        public async Task<bool> DeleteAsync(int orderId)
        {
            var order = await context.Orders.FindAsync(orderId);
            if (order != null)
            {
                context.Orders.Remove(order);
                await orderItemService.DeleteAsync(orderId);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
