using WebApplication1.Data.Entities;

namespace WebApplication1.Data.DTO
{
	public class UpdateProductDTO: IMergeable<UpdateProductDTO>
	{
		public string? Name { get; set; }
		public string? Description { get; set; }
		public decimal? Price { get; set; }
		public int? StockQuantity { get; set; }
	}
	public class AddProductDTO
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int StockQuantity { get; set; }
	}
}
