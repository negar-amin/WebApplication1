using WebApplication1.Data.Entities;

namespace WebApplication1.Data.DTO
{
	public class UpdateOrderDTO
	{
		public int? Id { get; set; }
		public User? User { get; set; }
		public Product? Product { get; set; }
		public DateTime? PurchaseTime { get; set; }
	}
	public class AddOrderRequestDTO
	{
		public int ProductId { get; set; }
		public int ProductCount { get; set; }
	}
}
