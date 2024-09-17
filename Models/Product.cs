using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RETOAPI.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Product { get; set; }
        public required string ProductCode { get; set; }
        public required string ProductName { get; set; }
        public required int CatProductId { get; set; }
        public  CategoryProduct CategoryProduct { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public bool ProductActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
