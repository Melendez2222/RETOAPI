using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RETOAPI.Models
{
    public class InvoiceDetail
    {
        [Key]
        public int ItemId { get; set; }
        [ForeignKey("Invoice")]
        public required int InvoiceID { get; set; }
        public virtual Invoice Invoice { get; set; }
        [ForeignKey("Product")]
        public required int ProductID { get; set; }
        public virtual Product Product { get; set; }
        public required string ProductName { get; set; }
        public required decimal ProductPrice { get; set; }
        public required int Quantity { get; set; }
        public required decimal SubtotalProduc { get; set; }
    }
}
