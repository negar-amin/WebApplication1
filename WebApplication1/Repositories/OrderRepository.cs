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

		public ICollection<Order> GetOrdersByUserId(int userId)
		{
			User user = _context.Users.Include(o => o.Orders).FirstOrDefault(o => o.Id == userId);
			if (user != null)
			{

				ICollection<Order> orders = new List<Order>();
				foreach (var order in user.Orders)
				{
					Order newOrder = _context.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == order.Id);
					orders.Add(newOrder);
				}
				return orders;
			}
			else
			{
				throw new Exception("user not found");
			}
		}
	}
}
