using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;

namespace WebApplication1.Services.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<Order> AddOrderAsync(int userId, List<AddOrderRequestDTO> productsInfo);
		public List<CustomerOrderDetailDTO> GetCustomerOrdersByDate(DateTime date);

	}
}
