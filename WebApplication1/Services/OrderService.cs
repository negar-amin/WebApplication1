using WebApplication1.Data.DTO;
using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public class OrderService : IOrderService
	{
		IRepository<Order> _ordersRepository;
		IProductService _productService;
		IUserService _userService;
		IProductOrderService _productOrderService;
		public OrderService(IRepository<Order> ordersRepository, IProductService productService, IUserService userService, IProductOrderService productOrderService)
        {
            _ordersRepository = ordersRepository;
			_productService = productService;
			_userService = userService;
			_productOrderService = productOrderService;
        }
        public async Task<Order> AddOrderAsync(int userId, List<AddOrderDTO> productsInfo)
		{
			var user = await _userService.GetUserByIdAsync(userId);
			ICollection<Product> products = new List<Product>();
			if (user == null)
			{
				throw new Exception("user or product doesn't exist");
			}
			foreach (var productInfo in productsInfo)
            {
				var product = await _productService.GetProductByIdAsync(productInfo.ProductId);
				if (productInfo.ProductCount < 1) throw new Exception("product count must be greater than 0");
				if (product == null) throw new Exception($"product with id {productInfo.ProductId} doesn't exist");
				if (product.StockQuantity < productInfo.ProductCount) throw new Exception($"stockQuantity wasn't enough.{product.StockQuantity} of {product.Name} is available at store");
				products.Add(product);
			}
            
            foreach (var productInfo in productsInfo)
            {
				var product = await _productService.GetProductByIdAsync(productInfo.ProductId);
				product.StockQuantity = product.StockQuantity-productInfo.ProductCount;
				_productService.UpdateProductAsync(product);
            }
            Order order = new Order
			{
				User = user,
				Products = products,
				PurchaseTime = DateTime.Now
			};
			var result = await _ordersRepository.AddAsync(order);
            foreach (var item in productsInfo)
            {
				Console.WriteLine(item);
				OrderProduct orderProduct = await _productOrderService.GetProductOrderAsync(result.Id, item.ProductId);
				orderProduct.ProductCount = item.ProductCount;
				_productOrderService.UpdateProductOrder(orderProduct);
            }
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
