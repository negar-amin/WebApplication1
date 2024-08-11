using HotChocolate.Authorization;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;
using WebApplication1.GraphQL.GraphQLResponseSchema;
using WebApplication1.Services.Contracts;

namespace WebApplication1.GraphQL.Mutation
{
    [ExtendObjectType(typeof(Mutation))]
	public class OrderMutations
	{
		public async Task<Response<Order>> AddOrder(int userId, List<AddOrderRequestDTO> productsInfo,[Service] IOrderService OrderService)
		{
			var response = await OrderService.AddOrderAsync(userId,productsInfo);
			return response;
		}
		public async Task<bool> DeleteOrder(int id, [Service]IOrderService orderService)
		{
			await orderService.DeleteOrderAsync(id);
			return true;
		}
	}
}
