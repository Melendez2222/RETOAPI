using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RETOAPI.Models
{
    public class Rols
    {
        [Key]
        public int RolId { get; set; }
        public required string RolName { get; set; }
        public bool RolActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<UserRole> UserRols { get; set; }
    }
}
