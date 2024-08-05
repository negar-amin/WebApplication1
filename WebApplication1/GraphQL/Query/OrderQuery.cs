using HotChocolate.Authorization;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Models;
using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1.GraphQL.Query
{
	[ExtendObjectType(typeof(Query))]
	public class OrderQuery
	{
		public async Task<IEnumerable<Order>> GetAllOrders([Service]IOrderService orderService)
		{
			return await orderService.GetAllOrdersAsync();
		}
		public async Task<Order> GetOrderById(int id,[Service] IOrderService orderService)
		{
			return await orderService.GetOrderByIdAsync(id);
		}
		[AllowAnonymous]
		public List<CustomerOrderDetailDTO> GetCustomerOrdersByDate(string date, [Service] IOrderCustomerService orderCustomerService)
		{
			DateTime d = Convert.ToDateTime(date);
			return orderCustomerService.GetCustomerOrdersByDate(d);
		}
	}
}
