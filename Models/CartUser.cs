using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RETOAPI.Models
{
    public class CartUser
    {
        [Key]
        public int IdCart { get; set; }
        [ForeignKey("Users")]
        public required int UserId { get; set; }
        public virtual Users Users { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<CartDetail> CartDetails { get; set; }
    }
}
