using System.ComponentModel.DataAnnotations;

namespace Cooktel_E_commrece.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Product> products { get; set; }

        public ICollection<Subcategory> subcategories { get; set; }
    }
}
