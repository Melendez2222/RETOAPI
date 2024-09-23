using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RETOAPI.Models
{
    public class Product
    {
        [Key]
        public int Id_Product { get; set; }
        public required string ProductCode { get; set; }
        public required string ProductName { get; set; }
        [ForeignKey("CategoryProduct")]
        public required int CatProductId { get; set; }
        public virtual CategoryProduct CategoryProduct { get; set; }
        public required decimal Price { get; set; }
        public required int Stock { get; set; }
        public bool ProductActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
