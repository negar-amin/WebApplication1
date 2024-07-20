using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;

public interface IProductService
{
	Task<IEnumerable<Product>> GetAllProductsAsync();
	Task<Product> GetProductByIdAsync(int id);
	Task AddProductAsync(Product product);
	Task UpdateProductAsync(Product product);
	Task DeleteProductAsync(int id);
	Task<bool> AddToStock(Product product, int count);
}
