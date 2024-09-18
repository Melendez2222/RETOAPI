namespace RETOAPI.DTOs
{
    public class ProductList
    {
        public int id_Product { get; set; }
        public string productCode {  get; set; }
        public string productName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
