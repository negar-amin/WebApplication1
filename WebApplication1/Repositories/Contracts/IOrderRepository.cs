using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;
using WebApplication1.GraphQL.GraphQLResponseSchema;

namespace WebApplication1.Repositories.Contracts
{
	public interface IOrderRepository:ICRUDRepository<Order>
	{
		List<CustomerOrderDetailDTO> GetAllOrdersInSpecialDate(DateTime date);
		Response<List<Order>> GetOrdersByUserId(int userId);
	}
}
