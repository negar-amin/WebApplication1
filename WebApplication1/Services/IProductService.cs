using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Models;

public interface IProductService
{
	Task<IEnumerable<Product>> GetAllProductsAsync();
	Task<Product> GetProductByIdAsync(int id);
	Task AddProductAsync(AddProductDTO product);
	Task UpdateProductAsync(int id, UpdateProductDTO input);
	Task DeleteProductAsync(int id);
	Task<bool> AddToStock(int productId, int count);
}
