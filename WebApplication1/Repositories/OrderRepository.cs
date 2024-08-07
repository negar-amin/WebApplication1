using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;
using WebApplication1.Repositories.Contracts;

namespace WebApplication1.Repositories
{
	public class OrderRepository : CRUDRepository<Order>, IOrderRepository
	{
        public OrderRepository(ApplicationDbContext context) : base(context) 
		{ 
		}

        public List<CustomerOrderDetailDTO> GetAllOrdersInSpecialDate(DateTime date)
		{
			List<CustomerOrderDetailDTO> output = new List<CustomerOrderDetailDTO>();
			var results = _context.Orders
				.Include(o => o.User)
				.Include(o => o.Products)
				.Where(o => o.PurchaseTime.Date ==  date.Date)
				.ToList();
			foreach (var result in results)
			{
				var middleResult = result.Adapt<CustomerOrderDetailDTO>();
				middleResult.ProductCollection = result.Products.ToList();
				output.Add(middleResult);
			}
			return  output;
		}
	}
}
