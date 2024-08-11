using GreenDonut;
using Mapster;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;
using WebApplication1.GraphQL.GraphQLResponseSchema;
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
        public async Task<Response<Order>> AddOrderAsync(int userId, List<AddOrderRequestDTO> productsInfo)
		{
			Response<Order> response = new Response<Order>();
			var user = await _userService.GetUserByIdAsync(userId);
			if (user == null)
			{
				response.Status.Add(ResponseError.NotFound with { Detail="user not found"});
				return response;
			}
			var products = await OrderIsPossible(productsInfo, response);
			if (response.Status.Count > 0) return response;
			UpdateOrderProductsStockQuantity(productsInfo);
            Order order = new Order
			{
				User = user,
				Products = products,
				PurchaseTime = DateTime.Now
			};
			var result = await _ordersRepository.AddAsync(order);
			AddProductCountOfOrderProductsToOrderProductTable(result, productsInfo);
			response.Data = order;
			return response;
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
		private async Task<ICollection<Product>> OrderIsPossible(List<AddOrderRequestDTO> productsInfo, Response<Order> response)
		{
			ICollection<Product> products = new List<Product>();
			foreach (var productInfo in productsInfo)
			{
				var product = await _productService.GetProductByIdAsync(productInfo.ProductId);
				if (productInfo?.ProductCount < 1) response.Status.Add(ResponseError.NegativeCount with { Detail="product count can't be negative"});
				if (product == null) response.Status.Add(ResponseError.NotFound with { Detail= $"product with id {productInfo.ProductId} doesn't exist" });
				if (product?.StockQuantity < productInfo.ProductCount) response.Status.Add(ResponseError.InadiquateStock with { Detail= $"only {product.StockQuantity} of {product.Name} is available at store" });
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
