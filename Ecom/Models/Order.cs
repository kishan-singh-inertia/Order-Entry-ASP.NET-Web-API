using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecom.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int OrderId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Required]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Discount { get; set; } = 0;
    }
}
