using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
	private readonly IProductService _productService;

	public ProductsController(IProductService productService)
	{
		_productService = productService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
	{
		var products = await _productService.GetAllProductsAsync();
		return Ok(products);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Product>> GetProduct(int id)
	{
		var product = await _productService.GetProductByIdAsync(id);
		if (product == null)
		{
			return NotFound();
		}
		return Ok(product);
	}

	[HttpPost]
	public async Task<ActionResult> CreateProduct(Product product)
	{
		await _productService.AddProductAsync(product);
		return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
	}

	[HttpPut("{id}")]
	public async Task<ActionResult> UpdateProduct(int id, Product product)
	{
		if (id != product.Id)
		{
			return BadRequest();
		}
		await _productService.UpdateProductAsync(product);
		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteProduct(int id)
	{
		await _productService.DeleteProductAsync(id);
		return NoContent();
	}
}
