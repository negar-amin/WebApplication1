using GreenDonut;
using Mapster;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;
using WebApplication1.Repositories;
using WebApplication1.Repositories.Contracts;
using WebApplication1.Services.Contracts;

namespace WebApplication1.Services
{
	public class OrderService : IOrderService
	{
		IOrderRepository _ordersRepository;
		IProductService _productService;
		IUserService _userService;
		IProductOrderService _productOrderService;
		public OrderService(IOrderRepository ordersRepository, IProductService productService, IUserService userService, IProductOrderService productOrderService)
        {
            _ordersRepository = ordersRepository;
			_productService = productService;
			_userService = userService;
			_productOrderService = productOrderService;
        }
        public async Task<Order> AddOrderAsync(int userId, List<AddOrderRequestDTO> productsInfo)
		{
			var user = await _userService.GetUserByIdAsync(userId);
			if (user == null)
			{
				throw new Exception("user doesn't exist");
			}
			var products = await OrderIsPossible(productsInfo);
			UpdateOrderProductsStockQuantity(productsInfo);
            Order order = new Order
			{
				User = user,
				Products = products,
				PurchaseTime = DateTime.Now
			};
			var result = await _ordersRepository.AddAsync(order);
			AddProductCountOfOrderProductsToOrderProductTable(result, productsInfo);
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
		private async Task<ICollection<Product>> OrderIsPossible(List<AddOrderRequestDTO> productsInfo)
		{
			ICollection<Product> products = new List<Product>();
			foreach (var productInfo in productsInfo)
			{
				var product = await _productService.GetProductByIdAsync(productInfo.ProductId);
				if (productInfo.ProductCount < 1) throw new Exception("product count must be greater than 0");
				if (product == null) throw new Exception($"product with id {productInfo.ProductId} doesn't exist");
				if (product.StockQuantity < productInfo.ProductCount) throw new Exception($"stockQuantity wasn't enough.{product.StockQuantity} of {product.Name} is available at store");
				products.Add(product);
			}
			return products;
		}

		private async void UpdateOrderProductsStockQuantity(List<AddOrderRequestDTO> productsInfo)
		{
			foreach (var productInfo in productsInfo)
			{
				var product = await _productService.GetProductByIdAsync(productInfo.ProductId);
				product.StockQuantity = product.StockQuantity - productInfo.ProductCount;
				await _productService.UpdateProductAsync(product.Id,product.Adapt<UpdateProductDTO>());
			}
		}

		private async void AddProductCountOfOrderProductsToOrderProductTable(Order addedOrder, List<AddOrderRequestDTO> productsInfo)
		{
			foreach (var item in productsInfo)
			{
				OrderProduct orderProduct = await _productOrderService.GetProductOrderAsync(addedOrder.Id, item.ProductId);
				orderProduct.ProductCount = item.ProductCount;
				await _productOrderService.UpdateProductOrder(orderProduct);
			}
		}

		public List<CustomerOrderDetailDTO> GetCustomerOrdersByDate(DateTime date)
		{
			return _ordersRepository.GetAllOrdersInSpecialDate(date);
		}
	}
}
