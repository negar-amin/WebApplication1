using WebApplication1.Data.Entities;

namespace WebApplication1.Services.Contracts
{
    public interface IProductOrderService
    {
        Task<OrderProduct> GetProductOrderAsync(int pk1, int pk2);
        Task UpdateProductOrder(OrderProduct product);
        Task<IEnumerable<OrderProduct>> GetAllProductOrdersAsync();
    }
}
