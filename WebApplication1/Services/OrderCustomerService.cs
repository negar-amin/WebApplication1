
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication1.Data.DTO;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
	public class OrderCustomerService : IOrderCustomerService
	{
		IOrderQueryRepository _orderRepository;
        public OrderCustomerService(IOrderQueryRepository orderQueryRepository)
        {
            _orderRepository = orderQueryRepository;
        }
        public List<OrderDetailDto> GetCustomerOrdersByDate(DateTime date)
		{
			return _orderRepository.GetOrderWithCustomer(date);
		}
	}
}
