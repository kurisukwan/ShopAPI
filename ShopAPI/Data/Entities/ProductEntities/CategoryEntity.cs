using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopAPI.Data.Entities.ProductEntities
{
    public class CategoryEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string CategoryName { get; set; } = null!;
        public virtual ICollection<ProductEntity> Product { get; set; }
    }
}
