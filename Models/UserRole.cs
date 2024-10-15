using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RETOAPI.Models
{
    public class UserRole
    {
        [Key]
        public int idrelation { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        public virtual Users Users { get; set; }
        [ForeignKey("Rols")]
        public int RolId { get; set; }
        public virtual Rols Rols { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
