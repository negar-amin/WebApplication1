using WebApplication1.Data.DTO;
using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public interface IOrderService
	{
		Task<IEnumerable<Order>> GetAllOrdersAsync();
		Task<Order> GetOrderByIdAsync(int id);
		Task UpdateOrderAsync(Order order);
		Task DeleteOrderAsync(int id);
		Task<Order> AddOrderAsync(int userId, List<AddOrderDTO> productsInfo);

	}
}
