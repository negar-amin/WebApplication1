using Mapster;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Models;
using WebApplication1.GraphQL.Subscription;
using WebApplication1.Services;

namespace WebApplication1.GraphQL.Mutation
{
	[ExtendObjectType(Name = "Mutation")]
	public class ProductMutation
	{
		public async Task<Product> AddProduct(AddProductDTO input, [Service] IProductService ProductService)
		{
			Product product = input.Adapt<Product>();
			await ProductService.AddProductAsync(product);
			return product;
		}
		public async Task<Product> UpdateProduct(int id, UpdateProductDTO input, [Service] IProductService ProductService)
		{
			Product product = await ProductService.GetProductByIdAsync(id);
			if (product == null)
			{
				throw new Exception("Product not found");
			}
			input.Adapt(product);
			await ProductService.UpdateProductAsync(product);
			return product;
		}
		public async Task<bool> AddToStock(int productId, int productCount, [Service] INotificationCRUD notificationCRUD, [Service] ICreateAddToStockNotification createAddToStockNotification, [Service] INotificationService notificationService, [Service] IProductService productService, [Service] IUserService userService)
		{
			Product product = await productService.GetProductByIdAsync(productId);
			var result = await productService.AddToStock(product, productCount);
			var notifications = await createAddToStockNotification.CreateNotification(product.Name, productCount);
			foreach (var notification in notifications)
			{
				await notificationCRUD.AddNotificationAsync(notification);
				await notificationService.SendNotification(nameof(ProductNotification.AddedToStock), notification);
			}
			return result;

		}
		public async Task<bool> DeleteProduct(int id, [Service] IProductService ProductService)
		{
			await ProductService.DeleteProductAsync(id);
			return true;
		}
	}
}
