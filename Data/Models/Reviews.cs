using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cooktel_E_commrece.Data.Models
{
    public class Reviews
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Comment { get; set; }

        [Required]
        DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public int Product_ID { get; set; }

        [ForeignKey("Product_ID")]
        public Product Product { get; set; }

        public Guid User_ID { get; set; }
        [ForeignKey("User_ID")]
        public User User { get; set; }  

    }
}
