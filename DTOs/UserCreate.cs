namespace RETOAPI.DTOs
{
    public class UserCreate
    {
        public required string UserRucDni { get; set; }
        public required string UserName { get; set; }
        public required string UserAddress { get; set; }
        public required string UserEmail { get; set; }
        public required string UserPhone { get; set; }
        public required string UserUsername { get; set; }
        public required string UserPassword { get; set; }
        public required int Rolid { get; set; }
    }
}
