using System.ComponentModel.DataAnnotations.Schema;

namespace Cooktel_E_commrece.Data.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<CartItems> cartItems { get; set; }
    }
}
