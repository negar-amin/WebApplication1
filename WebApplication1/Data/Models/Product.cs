﻿namespace WebApplication1.Data.Models
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int StockQuantity { get; set; }
		public ICollection<Order> Orders { get; set; }
		public ICollection<OrderProduct> ProductOrders { get; set; }

	}
}