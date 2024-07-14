using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public interface IProductOrderService
	{
		Task<OrderProduct> GetProductOrderAsync(int pk1, int pk2);
		Task UpdateProductOrder(OrderProduct product);
		Task<IEnumerable<OrderProduct>> GetAllProductOrdersAsync();
	}
}
