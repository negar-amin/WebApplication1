using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;
using WebApplication1.GraphQL.GraphQLResponseSchema;

namespace WebApplication1.Services.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<Response<Order>> AddOrderAsync(int userId, List<AddOrderRequestDTO> productsInfo);
		public List<CustomerOrderDetailDTO> GetCustomerOrdersByDate(DateTime date);

	}
}
