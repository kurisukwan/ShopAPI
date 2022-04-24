using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopAPI.Data.Entities.UserEntities
{
    public class AddressEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string Street { get; set; } = null!;
        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string PostalCode { get; set; } = null!;
        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string City { get; set; } = null!;
        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string Country { get; set; } = null!;
        public virtual ICollection<UserEntity> Users { get; set; }
    }
}
