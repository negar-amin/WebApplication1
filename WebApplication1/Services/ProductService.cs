using Mapster;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;
using WebApplication1.GraphQL.GraphQLResponseSchema;

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

	public async Task AddProductAsync(AddProductDTO input)
	{
		Product product = input.Adapt<Product>();
		await _productRepository.AddAsync(product);
	}

	public async Task<Response<Product>> UpdateProductAsync(int id, UpdateProductDTO input)
	{
		Response<Product> response = new Response<Product>();
		Product product =await GetProductByIdAsync(id);
		if (product == null)
		{
			response.Status.Add(ResponseError.NotFound with { Detail = $"Product with id {id} doesn't exist" });
			return response;
		}
		input.Adapt(product);
		await _productRepository.UpdateAsync(product);
		response.Data = product;
		return response;
	}

	public async Task DeleteProductAsync(int id)
	{
		await _productRepository.DeleteAsync(id);
	}

	public async Task<Response<Product>> AddToStock(int productId, int count)
	{
		Response<Product> response = new Response<Product>();
		Product product = await GetProductByIdAsync(productId);
		if (product == null) response.Status.Add(ResponseError.NotFound with { Detail = $"Product with id {productId} doesn't exist"});
		if (count <= 0) response.Status.Add(ResponseError.NegativeCount);
		if(response.Status.Count==0)
		{
			product.StockQuantity = product.StockQuantity + count;
			await UpdateProductAsync(product.Id, product.Adapt<UpdateProductDTO>());
			response.Data = product;
		}
		return response;
	}
}
