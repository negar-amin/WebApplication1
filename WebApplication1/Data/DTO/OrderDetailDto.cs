using WebApplication1.Data.Models;

namespace WebApplication1.Data.DTO
{
	public class OrderDetailDto
	{
		public Product Product { get; set; }
		public string Name { get; set; }
		public DateTime PurchaseTime { get; set; }
	}
}
