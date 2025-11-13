using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecom.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [MaxLength(100)]
        public required string ProductName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public required decimal Price { get; set; }
    }
}
