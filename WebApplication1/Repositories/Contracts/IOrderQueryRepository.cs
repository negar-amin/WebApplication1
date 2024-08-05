using WebApplication1.Data.DTO;
using WebApplication1.Data.Entities;

namespace WebApplication1.Repositories.Contracts
{
    public interface IOrderQueryRepository
    {
        List<CustomerOrderDetailDTO> GetAllOrdersInSpecialDate(DateTime date);
        ICollection<Order> GetOrdersByUserId(int userId);
    }
}
