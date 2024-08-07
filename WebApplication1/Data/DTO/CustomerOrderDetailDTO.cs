using WebApplication1.Data.Entities;

namespace WebApplication1.Data.DTO
{
	public class CustomerOrderDetailDTO
	{
		public List<Product>? ProductCollection { get; set; }
		public string BuyerName { get; set; }
		public DateTime PurchaseTime { get; set; }
	}
}
