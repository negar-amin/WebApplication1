using WebApplication1.Data.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
	public class ProductOrderService : IProductOrderService
	{
		private readonly IJoinTableRepository<OrderProduct> _joinTableRepository;
        public ProductOrderService(IJoinTableRepository<OrderProduct> joinTableRepository)
        {
            _joinTableRepository = joinTableRepository;
        }
        public async Task<IEnumerable<OrderProduct>> GetAllProductOrdersAsync()
		{
			return await _joinTableRepository.GetAll();
		}

		public async Task<OrderProduct> GetProductOrderAsync(int pk1 , int pk2)
		{
			return await _joinTableRepository.GetByPrimaryKey(pk1, pk2);
		}

		public async Task UpdateProductOrder(OrderProduct product)
		{
			await _joinTableRepository.UpdateRecord(product);
		}
	}
}
