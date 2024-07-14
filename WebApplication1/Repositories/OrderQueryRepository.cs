
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
						where order.PurchaseTime.Date == date.Date
						select new
						{
							order.Products,
							order.PurchaseTime,
							customer.FirstName,
							customer.LastName
						}
						;
			var result = query.ToList();
			List<OrderDetailDto> parsedResult = new List<OrderDetailDto>();
			foreach (var item in result)
			{
				var row = new OrderDetailDto
				{
					ProductCollection = item.Products,
					PurchaseTime = item.PurchaseTime,
					BuyerName = $"{item.FirstName} {item.LastName}"
				};
				parsedResult.Add(row);
			}
			return parsedResult;
		}
	}
}
