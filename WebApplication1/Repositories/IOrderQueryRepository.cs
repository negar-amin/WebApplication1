using WebApplication1.Data.DTO;

namespace WebApplication1.Repositories
{
	public interface IOrderQueryRepository
	{
		List<CustomerOrderDetailDTO> GetOrderWithCustomer(DateTime date);
	}
}
