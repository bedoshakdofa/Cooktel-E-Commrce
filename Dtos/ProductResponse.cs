using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Cooktel_E_commrece.Dtos
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        [Precision(10, 2)]
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
        public List<string> Image { get; set; } = new List<string>();

        [Required]
        public int ProductStock { get; set; }
        public CategoryDto Category { get; set; }
    }
}
