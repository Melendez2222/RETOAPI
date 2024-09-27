using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RETOAPI.Models
{
    public class CartDetail
    {
        [Key]
        public required int IdItemCart {  get; set; }
        [ForeignKey("CartUser")]
        public required int IdCart { get; set; }
        public virtual CartUser CartUser { get; set; }
        [ForeignKey("Product")]
        public required int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public DateTime CreateAt { get; set; }= DateTime.Now;

    }
}
