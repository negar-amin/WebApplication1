using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Data.Models;

namespace WebApplication1.Data.Configurations
{
	public class OrderCofiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.ToTable(nameof(Order));
			builder.HasOne(e => e.User)
				.WithMany(e => e.Orders)
				.HasForeignKey(e => e.UserId)
				.IsRequired();
		}
	}
}
