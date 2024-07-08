using WebApplication1.Data.Models;

namespace WebApplication1.Data.DTO
{
	public class UpdateOrderDto
	{
		public int? Id { get; set; }
		public User? User { get; set; }
		public Product? Product { get; set; }
		public DateTime? PurchaseTime { get; set; }
	}
	public class AddOrderDto
	{
		public User User { get; set; }
		public Product Product { get; set; }
		public DateTime PurchaseTime { get; set; }
	}
}
