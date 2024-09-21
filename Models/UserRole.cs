using System.ComponentModel.DataAnnotations;

namespace RETOAPI.Models
{
    public class UserRole
    {
        [Key]
        public int idrelation { get; set; }
        public int UserId { get; set; }
        public Users Users { get; set; }
        public int RolId { get; set; }
        public Rols Rols { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
