using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;

namespace WebApplication1.GraphQL.Query {
	[ExtendObjectType(typeof(Query))]
	public class ProductQueries
	{
		public Task<IEnumerable<Product>> GetProducts([Service] IProductService ProductService)
		{
			return ProductService.GetAllProductsAsync();
		}

		public Task<Product> GetProductById(int id, [Service] IProductService ProductService)
		{
			return ProductService.GetProductByIdAsync(id);
		}

	}
}