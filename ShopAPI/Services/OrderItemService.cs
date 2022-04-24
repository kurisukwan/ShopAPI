using Microsoft.EntityFrameworkCore;
using ShopAPI.Data;
using ShopAPI.Data.Entities.OrderEntities;
using ShopAPI.Models;
using ShopAPI.Models.Forms;

namespace ShopAPI.Services
{
    public interface IOrderItemService
    {
        Task<OrderItem> CreateAsync(TheItemForm form);
        Task<IEnumerable<OrderItem>> GetAllAsync(int orderId);
        Task<OrderItem> UpdateAsync(TheItemForm form);
        Task DeleteAsync(int orderId);
        Task<bool> DeleteItemAsync(DeleteForm form);
    }

    public class OrderItemService : IOrderItemService
    {
        private readonly DataContext context;

        public OrderItemService(DataContext context)
        {
            this.context = context;
        }

        // Create-----------------------------------------------------------------------------------
        public async Task<OrderItem> CreateAsync(TheItemForm form)
        {
            // Det måste finnas en sådan produkt och summan av antalet får inte vara mindre än 0.
            var product = await context.Products.FirstOrDefaultAsync(x => x.ArticleNumber == form.ArticleNumber);
            // Behövs för att kontrollera att en order har skapats först. Annars går det inte att lägga till produkten.
            var order = await context.Orders.FirstOrDefaultAsync(x => x.Id == form.OrderId);
            if (product != null && product.Quantity - form.Quantity != 0 && order != null)
            {
                var orderItemEntity = new OrderItemEntity
                {
                    OrderId = form.OrderId,
                    Quantity = form.Quantity,
                    ProductId = form.ArticleNumber
                };
                context.OrderItems.Add(orderItemEntity);
                await context.SaveChangesAsync();
                return new OrderItem(product.ProductName, product.Description, product.Price, orderItemEntity.Quantity);
            }
            return null!;
        }

        // ReadAll----------------------------------------------------------------------------------

        public async Task<IEnumerable<OrderItem>> GetAllAsync(int id)
        {
            var orderItems = new List<OrderItem>();
            foreach (var item in await context.OrderItems.Where(x => x.OrderId == id).Include(x => x.Product).ToListAsync())
            {
                orderItems.Add(new OrderItem
                {
                    ArticleNumber = item.ProductId,
                    OrderId = item.OrderId,
                    ProductName = item.Product.ProductName,
                    Description = item.Product.Description,
                    Price = item.Product.Price * item.Quantity,
                    Quantity = item.Quantity
                });
            }
            return orderItems;
        }

        // Update------------------------------------------------------------------------------------
        public async Task<OrderItem> UpdateAsync(TheItemForm form)
        {
            var orderItem = await context.OrderItems.Include(x => x.Product).FirstOrDefaultAsync(x => (x.OrderId == form.OrderId) && (x.ProductId == form.ArticleNumber));
            if (orderItem != null && orderItem.Product.Quantity - form.Quantity > 0)
            {
                orderItem.Quantity = form.Quantity;
                context.Entry(orderItem).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return new OrderItem(orderItem.Product.ProductName, orderItem.Product.Description, orderItem.Product.Price, orderItem.Quantity);
            }
            return null!;
        }

        // DeleteAll---------------------------------------------------------------------------------
        public async Task DeleteAsync(int orderId)
        {
            foreach (var item in await context.OrderItems.Where(x => x.OrderId == orderId).ToListAsync())
            {
                context.OrderItems.Remove(item);
            }
            await context.SaveChangesAsync();
        }

        // DeleteSingel------------------------------------------------------------------------------
        public async Task<bool> DeleteItemAsync(DeleteForm form)
        {
            var orderItem = await context.OrderItems.FirstOrDefaultAsync(x => (x.OrderId == form.OrderId) && ( x.ProductId == form.ArticleNumber));
            if (orderItem != null)
            {
                context.OrderItems.Remove(orderItem);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
