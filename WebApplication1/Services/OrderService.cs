using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public class OrderService : IOrderService
	{
		IRepository<Order> _ordersRepository;
		IProductService _productService;
		IUserService _userService;

		public OrderService(IRepository<Order> ordersRepository, IProductService productService, IUserService userService)
        {
            _ordersRepository = ordersRepository;
			_productService = productService;
			_userService = userService;
        }
        public async Task<Order> AddOrderAsync(int userId, int productId)
		{
			var user = await _userService.GetUserByIdAsync(userId);
			var product = await _productService.GetProductByIdAsync(productId);
			if (product == null || user == null)
			{
				throw new Exception("user or product doesn't exist");
			}
			Order order = new Order
			{
				User = user,
				Product = product,
				PurchaseTime = DateTime.Now
			};
			await _ordersRepository.AddAsync(order);
			return order;
		}

		public async Task DeleteOrderAsync(int id)
		{
			await _ordersRepository.DeleteAsync(id);
		}

		public Task<IEnumerable<Order>> GetAllOrdersAsync()
		{
			return _ordersRepository.GetAllAsync();
		}

		public async Task<Order> GetOrderByIdAsync(int id)
		{
			return await _ordersRepository.GetByIdAsync(id);
		}

		public async Task UpdateOrderAsync(Order order)
		{
			await _ordersRepository.UpdateAsync(order);
		}

	}
}
