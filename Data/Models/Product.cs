using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cooktel_E_commrece.Data.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        [Precision(10,2)]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(50)]
        public string color { get; set; }

        [Required]
        [MaxLength(25)]
        public string size { get; set; }

        [Required]
        [MaxLength(50)]
        public string Kind { get; set; }

        [Required]
        public List<string> Image { get; set; }= new List<string>();

        [Required]
        public int ProductStock { get; set; }

        [Required]
        public int SubCategoryID { get; set; }

        [ForeignKey("SubCategoryID")]
        public Subcategory subcategory { get; set; }

        public ICollection<Reviews>Reviews { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
        public ICollection<CartItems> CartItems { get; set; }
    }
}
