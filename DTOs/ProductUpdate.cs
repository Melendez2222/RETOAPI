using RETOAPI.Models;

namespace RETOAPI.DTOs
{
    public class ProductUpdate
    {
        public int Id_Product { get; set; }
        public required string ProductCode { get; set; }
        public required string ProductName { get; set; }
        public required int CatProductId { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public bool ProductActive { get; set; } = true;
    }
}
