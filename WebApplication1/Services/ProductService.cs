using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.Models;

public class ProductService : IProductService
{
	private readonly ICRUDRepository<Product> _productRepository;

	public ProductService(ICRUDRepository<Product> productRepository)
	{
		_productRepository = productRepository;
	}

	public async Task<IEnumerable<Product>> GetAllProductsAsync()
	{
		return await _productRepository.GetAllAsync();
	}

	public async Task<Product> GetProductByIdAsync(int id)
	{
		return await _productRepository.GetByIdAsync(id);
	}

	public async Task AddProductAsync(Product product)
	{
		await _productRepository.AddAsync(product);
	}

	public async Task UpdateProductAsync(Product product)
	{
		await _productRepository.UpdateAsync(product);
	}

	public async Task DeleteProductAsync(int id)
	{
		await _productRepository.DeleteAsync(id);
	}

	public async Task<bool> AddToStock(Product product, int count)
	{
		if (product == null)
		{
			throw new Exception("product doesn't exist");
		}
		if (count <= 0)
		{
			throw new Exception("product count must be greater than zero");
		}
		else
		{
			product.StockQuantity = product.StockQuantity + count;
			await UpdateProductAsync(product);
		}
		return true;
	}
}
