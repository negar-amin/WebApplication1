using WebApplication1.Data.Models;

namespace WebApplication1.Data.DTO
{
	public class CustomerOrderDetailDTO
	{
		public ICollection<Product> ProductCollection { get; set; }
		public string BuyerName { get; set; }
		public DateTime PurchaseTime { get; set; }
	}
}
