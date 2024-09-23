using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RETOAPI.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public int InvoiceNumber { get; set; }
        public required string ClientID { get; set; }
        public required string EmployeeID { get; set; }
        public required decimal Subtotal {  get; set; }  
        public required decimal PercentageIGV { get; set; }
        public decimal IGV { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
