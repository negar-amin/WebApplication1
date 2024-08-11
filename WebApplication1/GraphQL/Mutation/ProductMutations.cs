using Mapster;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;
using WebApplication1.GraphQL.GraphQLResponseSchema;
using WebApplication1.GraphQL.Subscription;
using WebApplication1.Services.Contracts;

namespace WebApplication1.GraphQL.Mutation
{
    [ExtendObjectType(typeof(Mutation))]
	public class ProductMutations
	{
		public async Task<bool> AddProduct(AddProductDTO input, [Service] IProductService ProductService)
		{
			await ProductService.AddProductAsync(input);
			return true;
		}
		public async Task<Response<Product>> UpdateProduct(int id, UpdateProductDTO input, [Service] IProductService ProductService)
		{
			var response = await ProductService.UpdateProductAsync(id,  input);
			return response;
		}
		public async Task<Response<Product>> AddToStock(int productId, int productCount, [Service] INotificationService notificationService, [Service] IProductService productService)
		{
			var result = await productService.AddToStock(productId, productCount);
			List<Notification> notifications = (List<Notification>)await notificationService.CreateAddToStockNotification(result.Data?.Name,productCount);
			await notificationService.SendNotification(nameof(Subscription.ProductNotification.AddedToStock),notifications);
			return result;

		}
		public async Task<bool> DeleteProduct(int id, [Service] IProductService ProductService)
		{
			await ProductService.DeleteProductAsync(id);
			return true;
		}
	}
}
