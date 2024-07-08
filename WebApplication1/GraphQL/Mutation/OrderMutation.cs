using WebApplication1.Data.DTO;
using WebApplication1.Data.Models;
using WebApplication1.Services;

namespace WebApplication1.GraphQL.Mutation
{
	[ExtendObjectType(Name = "Mutation")]
	public class OrderMutation
	{
		public async Task<Order> AddOrder(int userId, int productId,[Service] IOrderService OrderService, [Service] IProductService productService)
		{
			var order = await OrderService.AddOrderAsync(userId,productId);
			if (order != null) {
				Product product = order.Product;
				product.StockNumber=product.StockNumber-1;
				productService.UpdateProductAsync(product);
			}
			return order;
		}
		public async Task<bool> DeleteOrder(int id, [Service]IOrderService orderService)
		{
			await orderService.DeleteOrderAsync(id);
			return true;
		}
	}
}
