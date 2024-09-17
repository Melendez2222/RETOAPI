using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RETOAPI.Models
{
    public class InvoiceDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        public required int InvoiceID { get; set; }
        public Invoice Invoice { get; set; }
        public required int ProductID { get; set; }
        public Product Product { get; set; }
        public required string ProductName { get; set; }
        public required decimal ProductPrice { get; set; }
        public required int Quantity { get; set; }
        public required decimal SubtotalProduc { get; set; }
    }
}
