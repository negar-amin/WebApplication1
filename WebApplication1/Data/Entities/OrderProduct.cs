namespace WebApplication1.Data.Entities
{
	public class OrderProduct
	{
		public int ProductCount { get; set; }
		public int ProductId { get; set; }
		public int OrderId { get; set; }
		public Product Product { get; set; }
		public Order Order { get; set; }

	}
}
