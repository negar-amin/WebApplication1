namespace WebApplication1.Data.Entities
{
    public class Order 
    {
        public int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<OrderProduct>  OrderProducts { get; set; }
        public DateTime PurchaseTime { get; set; }
    }
}
