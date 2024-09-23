namespace RETOAPI.DTOs
{
    public class ProductCreate
    {
        public required string ProductCode { get; set; }
        public required string ProductName { get; set; }
        public required int CatId { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
    }
}
