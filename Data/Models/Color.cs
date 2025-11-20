using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cooktel_E_commrece.Data.Models
{
    public class Color
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string colorName {  get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

    }
}
