using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopAPI.Data.Entities.ProductEntities
{
    public class ProductEntity
    {
        [Key]
        public int ArticleNumber { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string ProductName { get; set; } = null!;
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = null!;
        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
    }
}
