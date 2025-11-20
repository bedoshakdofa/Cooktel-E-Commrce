using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cooktel_E_commrece.Data.Models
{
    public class OrderItems
    {
        public int Product_ID { get; set; }

        [ForeignKey("Product_ID")]
        public Product Product { get; set; }

        public int Order_ID { get; set; }
        [ForeignKey("Order_ID")]
        public Orders orders { get; set; }

        [Required]
        public int quantity { get; set; }
        [Required]
        public decimal quantity_per_unit { get; set; }
    }
}
