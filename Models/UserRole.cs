namespace RETOAPI.Models
{
    public class UserRole
    {
        public int UserId { get; set; }
        public Users Users { get; set; }
        public int RolId { get; set; }
        public Rols Rols { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
