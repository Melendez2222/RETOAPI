namespace RETOAPI.DTOs
{
    public class UserList
    {
        public int UserId { get; set; }
        public string UserRucDni { get; set; }
        public string UserName { get; set; }
        public string UserAddress { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public bool UserActive { get; set; } 
        public DateTime CreatedAt { get; set; }

    }
}
