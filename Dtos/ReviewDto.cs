using System.ComponentModel.DataAnnotations;

namespace Cooktel_E_commrece.Dtos
{
    public class ReviewDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
