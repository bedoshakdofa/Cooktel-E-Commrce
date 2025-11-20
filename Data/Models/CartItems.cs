using System.ComponentModel.DataAnnotations.Schema;

namespace Cooktel_E_commrece.Data.Models
{
    public class CartItems
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart cart { get; set; }

        public int quantity { get; set; }

        public Decimal unitPrice { get; set; }
    }
}
