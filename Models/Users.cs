using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RETOAPI.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public required string UserRucDni { get; set; }
        public required string UserName { get; set; }
        public required string UserAddress { get; set; }
        public required string UserEmail { get; set; }
        public required string UserPhone { get; set; }
        public required string UserUsername { get; set; }
        public required string UserPassword { get; set; }
        public int Attemp { get; set; } = 0;
        public bool UserActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public  virtual ICollection<UserRole> UserRols { get; set; }
        public virtual ICollection<CartUser> CartUsers{ get; set; }
    }
}
