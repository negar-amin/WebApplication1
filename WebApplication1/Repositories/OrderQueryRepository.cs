
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;
using WebApplication1.Repositories.Contracts;


namespace WebApplication1.Repositories
{
    public class OrderQueryRepository : IOrderQueryRepository
	{
		ApplicationDbContext _context;
        public OrderQueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<CustomerOrderDetailDTO> GetAllOrdersInSpecialDate(DateTime date)
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
			List<CustomerOrderDetailDTO> parsedResult = new List<CustomerOrderDetailDTO>();
			foreach (var item in result)
			{
				var row = new CustomerOrderDetailDTO
				{
					//ProductCollection = (List<Product>)item.Products,
					PurchaseTime = item.PurchaseTime,
					BuyerName = $"{item.FirstName} {item.LastName}"
				};
				parsedResult.Add(row);
			}
			return parsedResult;
		}

		public ICollection<Order> GetOrdersByUserId(int userId)
		{
			User user = _context.Users.Include(o=>o.Orders).FirstOrDefault(o => o.Id == userId);
			ICollection<Order> orders = new List<Order>();
            foreach (var order in user.Orders)
            {
                Order newOrder = _context.Orders.Include(o=>o.Products).FirstOrDefault(o=>o.Id == order.Id);
				orders.Add(newOrder);
            }
            if (user != null)
				return orders;
			else
				throw new Exception("user not found");
		}
	}
}
