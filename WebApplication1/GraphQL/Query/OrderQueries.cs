using HotChocolate.Authorization;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;
using WebApplication1.Repositories;
using WebApplication1.Services.Contracts;

namespace WebApplication1.GraphQL.Query
{
    [ExtendObjectType(typeof(Query))]
	public class OrderQueries
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
		public List<CustomerOrderDetailDTO> GetCustomerOrdersByDate(string date, [Service] IOrderService orderService)
		{
			DateTime d = Convert.ToDateTime(date);
			return  orderService.GetCustomerOrdersByDate(d);
		}
		public async Task<List<OrderProduct>> GetAllProductOrders([Service] IProductOrderService productOrderService)
		{
			return (List<OrderProduct>)await productOrderService.GetAllProductOrdersAsync();
		}
	}
}
