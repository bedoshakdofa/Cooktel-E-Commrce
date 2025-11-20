using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cooktel_E_commrece.Data.Models
{
    public class CardNumber
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CardName { get; set; }

        [Required]
        public string CardNum {  get; set; }

        [Required]

        public DateTime ExpDate { get; set; }

        [Required]
        public Guid UserID { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
