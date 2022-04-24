using ShopAPI.Data.Entities.ProductEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopAPI.Data.Entities.OrderEntities
{
    public class OrderItemEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual OrderEntity Order { get; set; }
        public virtual ProductEntity Product { get; set; }

    }
}
