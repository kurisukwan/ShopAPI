using Microsoft.EntityFrameworkCore;
using ShopAPI.Data.Entities.OrderEntities;
using ShopAPI.Data.Entities.ProductEntities;
using ShopAPI.Data.Entities.UserEntities;

namespace ShopAPI.Data
{
    public class DataContext : DbContext
    {
        protected DataContext()
        {
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<AddressEntity> Addresses { get; set; }
        public virtual DbSet<ProductEntity> Products { get; set; }
        public virtual DbSet<CategoryEntity> Categories { get; set; }
        public virtual DbSet<OrderEntity> Orders { get; set; }
        public virtual DbSet<OrderItemEntity> OrderItems { get; set; }
    }
}
