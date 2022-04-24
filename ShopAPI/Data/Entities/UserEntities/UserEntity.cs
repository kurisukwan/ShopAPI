using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopAPI.Data.Entities.UserEntities
{
    [Index(nameof(Email), IsUnique = true)]
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; } = null!;
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; } = null!;
        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string Email { get; set; } = null!;
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual AddressEntity Address { get; set; }
    }
}
