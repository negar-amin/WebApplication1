using WebApplication1.Data.DTO;

namespace WebApplication1.Repositories
{
	public interface IOrderQueryRepository
	{
		List<OrderDetailDto> GetOrderWithCustomer(DateTime date);
	}
}
