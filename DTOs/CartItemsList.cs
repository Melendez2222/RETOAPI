namespace RETOAPI.DTOs
{
    public class CartItemsList
    {
        public int IdItemCart { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateAt { get; set; }
    }   
}
