namespace RETOAPI.DTOs
{
    public class UserUpdate
    {
        public int UserId { get; set; }
        public required string UserRucDni { get; set; }
        public required string UserName { get; set; }
        public required string UserAddress { get; set; }
        public required string UserEmail { get; set; }
        public required string UserPhone { get; set; }
        public bool UserActive { get; set; }
        public required int Rolid { get; set; }
    }
}
