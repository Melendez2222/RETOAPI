namespace RETOAPI.DTOs
{
    public class RolUpdate
    {
        public int RolId { get; set; }
        public required string RolName { get; set; }
        public bool RolActive { get; set; }
    }
}
