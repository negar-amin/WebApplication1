using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;
using WebApplication1.GraphQL.GraphQLResponseSchema;

public interface IProductService
{
	Task<IEnumerable<Product>> GetAllProductsAsync();
	Task<Product> GetProductByIdAsync(int id);
	Task AddProductAsync(AddProductDTO product);
	Task<Response<Product>> UpdateProductAsync(int id, UpdateProductDTO input);
	Task DeleteProductAsync(int id);
	Task<Response<Product>> AddToStock(int productId, int count);
}
