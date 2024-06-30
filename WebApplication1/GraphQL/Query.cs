using WebApplication1.GraphQL;

public class Query
{
	public Task<IEnumerable<Product>> GetProducts([Service] IProductService ProductService)
	{
		return ProductService.GetAllProductsAsync();
	}

	public Task<Product> GetProductById(int id, [Service] IProductService ProductService)
	{
		return ProductService.GetProductByIdAsync(id);
	}
	public async Task<Product> AddProduct(ProductInput input, [Service] IProductService ProductService)
	{
		var product = new Product
		{
			Name = input.Name,
			Description = input.Description,
			Price = input.Price
		};
		await ProductService.AddProductAsync(product);
		return product;
	}
	public async Task<Product> UpdateProduct(int id, ProductInput input, [Service] IProductService ProductService)
	{
		var product = await ProductService.GetProductByIdAsync(id);
		if (product == null)
		{
			throw new Exception("Product not found");
		}
		product.Name = input.Name;
		product.Description = input.Description;
		product.Price = input.Price;
		await ProductService.UpdateProductAsync(product);
		return product;
	}
		public async Task<bool> DeleteProduct(int id, [Service] IProductService ProductService)
	{
			await ProductService.DeleteProductAsync(id);
			return true;
	}
}