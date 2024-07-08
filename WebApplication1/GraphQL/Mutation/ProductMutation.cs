using WebApplication1.Data.DTO;
using WebApplication1.Data.Models;

namespace WebApplication1.GraphQL.Mutation
{
	[ExtendObjectType(Name = "Mutation")]
	public class ProductMutation
	{
		public async Task<Product> AddProduct(AddProductDTO input, [Service] IProductService ProductService)
		{
			var product = new Product
			{
				Name = input.Name,
				Description = input.Description,
				Price = input.Price,
				StockNumber = input.StockNumber
			};
			await ProductService.AddProductAsync(product);
			return product;
		}
		public async Task<Product> UpdateProduct(int id, UpdateProductDTO input, [Service] IProductService ProductService)
		{
			var product = await ProductService.GetProductByIdAsync(id);
			if (product == null)
			{
				throw new Exception("Product not found");
			}
			product.Name = input.Name == null ? product.Name : input.Name;
			product.Description = input.Description == null ? product.Description : input.Description;
			product.Price = (decimal)(input.Price == null ? product.Price : input.Price);
			product.StockNumber = (int)(input.StockNumber == null ? product.StockNumber : input.StockNumber);
			await ProductService.UpdateProductAsync(product);
			return product;
		}
		public async Task<bool> DeleteProduct(int id, [Service] IProductService ProductService)
		{
			await ProductService.DeleteProductAsync(id);
			return true;
		}
	}
}
