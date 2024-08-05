using Mapster;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Models;
using WebApplication1.GraphQL.Subscription;
using WebApplication1.Services;

namespace WebApplication1.GraphQL.Mutation
{
	[ExtendObjectType(typeof(Mutation))]
	public class ProductMutation
	{
		public async Task<bool> AddProduct(AddProductDTO input, [Service] IProductService ProductService)
		{
			await ProductService.AddProductAsync(input);
			return true;
		}
		public async Task<bool> UpdateProduct(int id, UpdateProductDTO input, [Service] IProductService ProductService)
		{
			await ProductService.UpdateProductAsync(id,  input);
			return true;
		}
		public async Task<bool> AddToStock(int productId, int productCount, [Service] INotificationCRUD notificationCRUD, [Service] ICreateAddToStockNotification createAddToStockNotification, [Service] INotificationService notificationService, [Service] IProductService productService, [Service] IUserService userService)
		{
			var result = await productService.AddToStock(productId, productCount);
			//var notifications = await createAddToStockNotification.CreateNotification(product.Name, productCount);
			//foreach (var notification in notifications)
			//{
			//	await notificationCRUD.AddNotificationAsync(notification);
			//	await notificationService.SendNotification(nameof(ProductNotification.AddedToStock), notification);
			//}
			return result;

		}
		public async Task<bool> DeleteProduct(int id, [Service] IProductService ProductService)
		{
			await ProductService.DeleteProductAsync(id);
			return true;
		}
	}
}
