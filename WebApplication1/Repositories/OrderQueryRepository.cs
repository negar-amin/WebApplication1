
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Models;

namespace WebApplication1.Repositories
{
	public class OrderQueryRepository : IOrderQueryRepository
	{
		ApplicationDbContext _context;
        public OrderQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<OrderDetailDto> GetOrderWithCustomer(DateTime date)
		{
			var query = from order in _context.Orders
						join customer in _context.Users
						on order.User.Id equals customer.Id
						where order.PurchaseTime == date
						select new
						{
							order.Product,
							order.PurchaseTime,
							customer.Name,
						}
						;
			var result = query.ToList();
			List<OrderDetailDto> parsedResult = new List<OrderDetailDto>();
			foreach (var item in result)
			{
				var row = new OrderDetailDto
				{
					Product = item.Product,
					PurchaseTime = item.PurchaseTime,
					Name = item.Name
				};
				parsedResult.Add(row);
			}
			return parsedResult;
		}
	}
}
