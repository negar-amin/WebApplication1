using WebApplication1.Data.DTO;

namespace WebApplication1.Services
{
	public interface IOrderCustomerService
	{
		public List<CustomerOrderDetailDTO> GetCustomerOrdersByDate(DateTime date);
	}
}
