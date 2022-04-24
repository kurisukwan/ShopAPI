using ShopAPI.Data.Entities.ProductEntities;
using ShopAPI.Data.Entities.UserEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopAPI.Data.Entities.OrderEntities
{
    public class OrderEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; } = null!;
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public virtual ICollection<OrderItemEntity> OrderItems { get; set; }
    }
}
