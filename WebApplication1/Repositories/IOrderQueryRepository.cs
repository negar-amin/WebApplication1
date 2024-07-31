using WebApplication1.Data.DTO;
using WebApplication1.Data.Models;

namespace WebApplication1.Repositories
{
	public interface IOrderQueryRepository
	{
		List<CustomerOrderDetailDTO> GetAllOrdersInSpecialDate(DateTime date);
		ICollection<Order> GetOrdersByUserId(int  userId);
	}
}
