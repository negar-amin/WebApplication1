namespace WebApplication1.Data.DTO
{
	public class UpdateProductDTO
	{
		public int? Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public decimal? Price { get; set; }
		public int? StockNumber { get; set; }
	}
	public class AddProductDTO
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int StockNumber { get; set; }
	}
}
