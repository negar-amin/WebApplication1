namespace WebApplication1.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
        public DateTime PurchaseTime { get; set; }
    }
}
