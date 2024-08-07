using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;

namespace WebApplication1.Repositories.Contracts
{
	public interface IOrderRepository:ICRUDRepository<Order>
	{
		List<CustomerOrderDetailDTO> GetAllOrdersInSpecialDate(DateTime date);
	}
}
