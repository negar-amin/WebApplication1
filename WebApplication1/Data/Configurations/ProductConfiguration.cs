using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Data.Entities;

namespace WebApplication1.Data.Configurations
{
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.ToTable(nameof(Product));
			builder.HasMany(e => e.Orders)
				.WithMany(e => e.Products)
				.UsingEntity<OrderProduct>(
				l => l.HasOne(e => e.Order).WithMany(e => e.OrderProducts).HasForeignKey(e=>e.OrderId),
				l => l.HasOne(e=>e.Product).WithMany(e=>e.ProductOrders).HasForeignKey(e=>e.ProductId));
		}
	}
}
