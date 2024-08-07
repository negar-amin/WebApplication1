using WebApplication1.Data.Entities;
using WebApplication1.Repositories.Contracts;
using WebApplication1.Services.Contracts;

namespace WebApplication1.Services
{
    public class ProductOrderService : IProductOrderService
	{
		private readonly ICRUDRepository<OrderProduct> _repository;
        public ProductOrderService(ICRUDRepository<OrderProduct> repository)
        {
			_repository = repository;
        }
        public async Task<IEnumerable<OrderProduct>> GetAllProductOrdersAsync()
		{
			return await _repository.GetAllAsync();
		}

		public async Task<OrderProduct> GetProductOrderAsync(int pk1 , int pk2)
		{
			return await _repository.GetByIdAsync(pk1, pk2);
		}

		public async Task UpdateProductOrder(OrderProduct product)
		{
			await _repository.UpdateAsync(product);
		}
	}
}
